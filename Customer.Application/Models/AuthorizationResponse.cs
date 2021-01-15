using System;

namespace Customers.Application.Models
{
    public class AuthorizationResponse : StatusResponse
    {
        public bool IsAdmin { get; set; }
        
        public string Email { get; set; }

        public string AccessToken { get; set; }
        
        public DateTime AccessTokenExpiryDateUtc { get; set; }
        
        public string RefreshToken { get; set; }
        
        public DateTime RefreshTokenExpiryDateUtc { get; set; }
    }
}