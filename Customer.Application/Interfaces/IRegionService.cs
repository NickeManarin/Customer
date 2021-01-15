using System.Threading.Tasks;

namespace Customers.Application.Interfaces
{
    public interface IRegionService
    {
        Task<IResponse> GetRegions();
    }
}