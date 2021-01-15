using System.Linq;
using Customers.Data.Context;
using Customers.Domain.Interfaces;
using Customers.Domain.Models;

namespace Customers.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        
        public IQueryable<User> GetUsers()
        {
            return _context.Users;
        }
    }
}