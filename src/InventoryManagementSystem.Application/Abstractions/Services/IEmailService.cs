using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(string subject, string to, string body);
    }
}
