using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;

        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
