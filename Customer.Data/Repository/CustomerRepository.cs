using System.Linq;
using Customers.Data.Context;
using Customers.Domain.Interfaces;
using Customers.Domain.Models;

namespace Customers.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context)
        {
            _context = context;
        }


        public IQueryable<Gender> GetGenders()
        {
            return _context.Genders;
        }

        public IQueryable<Region> GetRegions()
        {
            return _context.Regions;
        }

        public IQueryable<City> GetCities()
        {
            return _context.Cities;
        }

        public IQueryable<Classification> GetClassifications()
        {
            return _context.Classifications;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return _context.Customers;
        }
    }
}