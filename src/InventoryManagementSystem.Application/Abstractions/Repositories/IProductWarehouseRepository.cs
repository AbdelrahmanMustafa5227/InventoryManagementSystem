using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Repositories
{
    public interface IProductWarehouseRepository
    {
        Task AddToStock(long productId , long warehouseId , int quantity);
        Task RemoveFromStock(long productId, long warehouseId, int quantity);
        Task TransferStock(long productId, long sourseWarehouseId, long destinationWarehouseId, int quantity);
        Task<ProductWarehouse?> GetByIdAsync(long productId , long warehouseId);
        Task<ProductWarehouse?> GetDetailedByIdAsync(long productId, long warehouseId);
    }
}
