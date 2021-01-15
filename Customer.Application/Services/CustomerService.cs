using System;
using System.Linq;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _repository;
        private readonly IUserRepository _userRepository;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository repository, IUserRepository userRepository)
        {
            _logger = logger;
            _repository = repository;
            _userRepository = userRepository;
        }

        
        public async Task<IResponse> GetCustomers(CustomerRequest request, string caller, bool isAdmin)
        {
            try
            {
                var query = _repository.GetCustomers().Include(i => i.Gender).Include(i => i.Classification).Include(i => i.User).Include(i => i.City).ThenInclude(t => t.Region).AsQueryable();

                if ((request.Name ?? "").Trim().Length > 0)
                    query = query.Where(w => w.Name.Contains(request.Name.Trim()));
                
                if (request.Gender.HasValue)
                    query = query.Where(w => w.GenderId == request.Gender.Value);
                
                if (request.Region.HasValue)
                    query = query.Where(w => w.City.RegionId == request.Region.Value);

                if (request.City.HasValue)
                    query = query.Where(w => w.CityId == request.City.Value);

                if (request.Classification.HasValue)
                    query = query.Where(w => w.ClassificationId == request.Classification.Value);

                if (request.PurchaseStart.HasValue && request.PurchaseEnd.HasValue)
                    query = query.Where(w => w.LastPurchase >= request.PurchaseStart.Value && w.LastPurchase <= request.PurchaseEnd.Value);

                if (isAdmin)
                {
                    if (request.Seller.HasValue)
                        query = query.Where(w => w.UserId == request.Seller.Value);
                }
                else
                {
                    query = query.Where(w => w.User.Email == caller);
                }

                var customers = await query.ToListAsync();

                foreach (var customer in customers)
                    customer.User = new User { Email = customer.User.Email };
                
                return new StatusResponse(200, 200, customers);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return new StatusResponse(500, 500, "Internal error.");
            }
        }
    }
}