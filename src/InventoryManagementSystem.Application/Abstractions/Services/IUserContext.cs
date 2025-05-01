using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Services
{
    public interface IUserContext
    {
        long GetLoggedUserId { get; }
        string GetLoggedUserEmail { get; }
    }
}
