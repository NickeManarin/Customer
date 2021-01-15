using System.Threading.Tasks;

namespace Customers.Application.Interfaces
{
    public interface IClassificationService
    {
        Task<IResponse> GetClassifications();
    }
}