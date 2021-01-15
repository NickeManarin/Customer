using System.Threading.Tasks;

namespace Customers.Application.Interfaces
{
    public interface ICityService
    {
        Task<IResponse> GetCities();
    }
}