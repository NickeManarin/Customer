using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Customers.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IUserRepository _repository;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ICacheManager<string> _cache;

        public AuthService(ILogger<CustomerService> logger, IUserRepository repository, IOptions<AppSettings> appSettings, ICacheManager<string> cache)
        {
            _logger = logger;
            _repository = repository;
            _appSettings = appSettings;
            _cache = cache;
        }
        
        
        public async Task<IResponse> SignIn(AuthorizationRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return new StatusResponse(400, 101, "Email or password missing.");

            var user = await _repository.GetUsers().Include(i => i.UserRole).SingleOrDefaultAsync(x => x.Email == request.Email);

            //Check if user exists.
            if (user == null)
                return new StatusResponse(400, 100, "User missing.");

            if (!IsPasswordHashValid(request.Password, user.PasswordHash, user.PasswordSalt))
                return new StatusResponse(400, 102, "Email or password invalid.");

            return await LogIn(user);
        }

        public async Task<IResponse> RefreshAccessToken(RefreshRequest request)
        {
            var user = _repository.GetUsers().Include(i => i.UserRole).FirstOrDefault(f => f.Email == request.Email);

            if (user == null)
                return new StatusResponse(400, 100, "User missing.");

            //Check if the refresh token is still valid.
            if (string.IsNullOrWhiteSpace(request.RefreshToken) || await Task.Factory.StartNew(() => _cache.Get(request.RefreshToken, request.Email) == null))
                return new StatusResponse(400, 106, "Refresh token invalid.");

            var response = CreateAccessToken(user);

            return new RefreshResponse
            {
                Status = 200,
                Code = 200,
                AccessToken = response.AccessToken,
                AccessTokenExpiryDateUtc = response.AccessTokenExpiryDateUtc
            };
        }

        public async Task<IResponse> RevokeRefreshToken(RefreshRequest request)
        {
            //This method can be called when SignOut or when a given token must be revoked manually.
            if (request.RefreshToken == null || request.Email == null || await Task.Factory.StartNew(() => _cache.Get(request.RefreshToken, request.Email)) == null)
                return new StatusResponse(200, 105, "Refresh token already invalidated.");

            await Task.Factory.StartNew(() => _cache.Remove(request.RefreshToken, request.Email));

            return new StatusResponse(200, 201, "Refresh token invalidated.");
        }

        public (string, bool) AdminTokenVerification(string token)
        {
            //Creates a new access token.
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.Replace("Bearer ", ""));

            //return !jwt.Payload.Iat.HasValue ? DateTime.MaxValue : DateTime.UnixEpoch.AddSeconds(jwt.Payload.Iat.Value);
            return (jwt.Claims.FirstOrDefault(f => f.Type == "email")?.Value, jwt.Claims.FirstOrDefault(f => f.Type == "role")?.Value == "Administrator");
        }
        

        private static bool IsPasswordHashValid(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                //Verifies if both hashes match.
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                    return false;
            }

            return true;
        }

        private async Task<IResponse> LogIn(User user)
        {
            //Creates the refresh token.
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            var refreshExpiration = DateTime.UtcNow.AddDays(50);

            //Creates access token.
            var response = CreateAccessToken(user);

            //Add token to cache, with expiration.
            await Task.Factory.StartNew(() => _cache.Add(new CacheItem<string>(refreshToken, user.Email, " ", ExpirationMode.Absolute, TimeSpan.FromDays(50))));

            response.IsAdmin = user.UserRole.IsAdmin;
            response.Email = user.Email;
            response.RefreshToken = refreshToken;
            response.RefreshTokenExpiryDateUtc = refreshExpiration;
            response.Status = 200;
            response.Code = 200;
            
            return response;
        }

        private AuthorizationResponse CreateAccessToken(User user)
        {
            //Creates a new access token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthorizationResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                AccessTokenExpiryDateUtc = tokenDescriptor.Expires.Value
            };
        }
    }
}