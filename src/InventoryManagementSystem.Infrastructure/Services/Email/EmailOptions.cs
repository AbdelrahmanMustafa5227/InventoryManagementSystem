using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Services.Email
{
    internal class EmailOptions
    {
        public const string SectionName = "EmailOptions";

        public string Host { get; set; } = string.Empty;
        public int PortNumber { get; set; }
        public bool UseSSL { get; set; }
    }
}
