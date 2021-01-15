using System;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Services
{
    public class GenderService : IGenderService
    {
        private readonly ILogger<GenderService> _logger;
        private readonly ICustomerRepository _repository;

        public GenderService(ILogger<GenderService> logger, ICustomerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        
        public async Task<IResponse> GetGenders()
        {
            try
            {
                return new StatusResponse(200, 200, await _repository.GetGenders().ToListAsync());
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return new StatusResponse(500, 500, "Internal error.");
            }
        }
    }
}