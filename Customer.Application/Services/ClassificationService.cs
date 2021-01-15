using System;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Services
{
    public class ClassificationService : IClassificationService
    {
        private readonly ILogger<ClassificationService> _logger;
        private readonly ICustomerRepository _repository;

        public ClassificationService(ILogger<ClassificationService> logger, ICustomerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        
        public async Task<IResponse> GetClassifications()
        {
            try
            {
                return new StatusResponse(200, 200, await _repository.GetClassifications().ToListAsync());
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return new StatusResponse(500, 500, "Internal error.");
            }
        }
    }
}