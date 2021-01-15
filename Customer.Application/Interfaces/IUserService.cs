using System.Threading.Tasks;

namespace Customers.Application.Interfaces
{
    public interface IUserService
    {
        Task<IResponse> GetUsers();
    }
}