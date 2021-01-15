using System.Threading.Tasks;
using Customers.Application.Models;

namespace Customers.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IResponse> GetCustomers(CustomerRequest request, string caller, bool isAdmin);
    }
}