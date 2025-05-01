using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Domain.Entities
{
    public class Warehouse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
    }
}
