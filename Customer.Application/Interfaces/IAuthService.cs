using System.Threading.Tasks;
using Customers.Application.Models;

namespace Customers.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IResponse> SignIn(AuthorizationRequest request);
        Task<IResponse> RefreshAccessToken(RefreshRequest request);
        Task<IResponse> RevokeRefreshToken(RefreshRequest request);
        (string, bool) AdminTokenVerification(string token);
    }
}