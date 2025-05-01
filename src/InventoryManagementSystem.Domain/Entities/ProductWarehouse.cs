using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Domain.Entities
{
    public class ProductWarehouse
    {
        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public long WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
