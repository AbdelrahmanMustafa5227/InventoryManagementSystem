using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Exist(long userId);
        Task<bool> Exist(string username, string email);
        Task<User?> GetByEmailAsync(string email);
        void Register(User user);
    }
}
