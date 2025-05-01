using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int LowStockThreshold { get; set; }

        public ICollection<ProductWarehouse> ProductWarehouse { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
    }
}
