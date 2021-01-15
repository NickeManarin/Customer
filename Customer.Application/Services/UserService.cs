using System;
using System.Threading.Tasks;
using Customers.Application.Interfaces;
using Customers.Application.Models;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repository;

        public UserService(ILogger<UserService> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        
        public async Task<IResponse> GetUsers()
        {
            try
            {
                return new StatusResponse(200, 200, await _repository.GetUsers().Include(i => i.UserRole).ToListAsync());
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return new StatusResponse(500, 500, "Internal error.");
            }
        }
    }
}