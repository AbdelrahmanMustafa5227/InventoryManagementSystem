using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories
{
    internal class ProductWarehouseRepository : IProductWarehouseRepository
    {
        private readonly AppDbContext _context;

        public ProductWarehouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductWarehouse?> GetDetailedByIdAsync(long productId, long warehouseId)
        {
            return await _context.ProductWarehouses
                .Include(x => x.Product)
                .Include(x => x.Warehouse)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId);
        }
        public async Task<IEnumerable<ProductWarehouse>> GetAllProductWarehousesAsync()
        {
            return await _context.ProductWarehouses
                .ToListAsync();
        }

        public async Task AddToStock(long productId, long warehouseId, int quantity)
        {
            var pwFromDb = await GetByIdAsync(productId, warehouseId);

            if (pwFromDb is null)
                _context.ProductWarehouses.Add(new ProductWarehouse { ProductId = productId, WarehouseId = warehouseId, Quantity = quantity });
            else
                pwFromDb.Quantity += quantity;
        }

        public async Task RemoveFromStock(long productId, long warehouseId, int quantity)
        {
            var pwFromDb = await GetByIdAsync(productId, warehouseId);
            if (pwFromDb is not null)
                pwFromDb.Quantity -= quantity;
        }

        public async Task TransferStock(long productId, long sourseWarehouseId, long destinationWarehouseId, int quantity)
        {
            await RemoveFromStock(productId, sourseWarehouseId, quantity);
            await AddToStock(productId, destinationWarehouseId, quantity);
        }

        public void DeleteProductWarehouse(ProductWarehouse productWarehouse)
        {
            _context.ProductWarehouses.Remove(productWarehouse);
        }

        public async Task<ProductWarehouse?> GetByIdAsync(long productId, long wareHouseId)
        {
            return await _context.ProductWarehouses.FirstOrDefaultAsync(x => x.ProductId == productId && x.WarehouseId == wareHouseId);
        }
    }
}
