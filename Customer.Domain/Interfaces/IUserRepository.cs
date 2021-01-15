using System.Linq;
using Customers.Domain.Models;

namespace Customers.Domain.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
    }
}