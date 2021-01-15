using System.Linq;
using Customers.Domain.Models;

namespace Customers.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        IQueryable<Gender> GetGenders();
        IQueryable<Region> GetRegions();
        IQueryable<City> GetCities();
        IQueryable<Classification> GetClassifications();
        IQueryable<Customer> GetCustomers();
    }
}