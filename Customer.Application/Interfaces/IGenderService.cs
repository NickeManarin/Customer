using System.Threading.Tasks;

namespace Customers.Application.Interfaces
{
    public interface IGenderService
    {
        Task<IResponse> GetGenders();
    }
}