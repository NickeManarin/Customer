using System;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Services
{
    public class RegionService : IRegionService
    {
        private readonly ILogger<RegionService> _logger;
        private readonly ICustomerRepository _repository;

        public RegionService(ILogger<RegionService> logger, ICustomerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        
        public async Task<IResponse> GetRegions()
        {
            try
            {
                return new StatusResponse(200, 200, await _repository.GetRegions().ToListAsync());
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return new StatusResponse(500, 500, "Internal error.");
            }
        }
    }
}