using InventoryManagementSystem.Application.Features.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Mappings
{
    public static class UserMappings
    {
        public static User ToModel(this RegisterCommand command)
        {
            return new User
            {
                Username = command.Username,
                Email = command.Email,
                Role = command.Role
            };
        }
    }
}
