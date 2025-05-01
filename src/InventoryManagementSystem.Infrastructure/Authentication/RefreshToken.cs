using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Authentication
{
    internal class RefreshToken
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
