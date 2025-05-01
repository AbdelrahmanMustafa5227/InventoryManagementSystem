using InventoryManagementSystem.Application.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Repositories
{
    public interface IWarehouseRepository
    {
        void Add(Warehouse warehouse);
        Task<bool> Exist(long warehouseId);
        Task<QueryResult<Warehouse>> GetWarehousesWithLowStockProducts(int page);
        Task<List<Warehouse>> GetWarehousesWithLowStockProducts();
    }
}
