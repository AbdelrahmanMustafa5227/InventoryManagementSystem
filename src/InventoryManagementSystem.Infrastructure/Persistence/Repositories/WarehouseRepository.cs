using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories
{
    internal class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _context;
        private readonly int _pageSize;

        public WarehouseRepository(AppDbContext context , IOptions<PaginationOptions> options)
        {
            _context = context;
            _pageSize = options.Value.PageSize;
        }

        public void Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
        }

        public Task<bool> Exist(long warehouseId)
        {
            return _context.Warehouses.AnyAsync(x => x.Id == warehouseId);
        }

        public async Task<QueryResult<Warehouse>> GetWarehousesWithLowStockProducts(int page)
        {
            var count = await _context.Warehouses.Where(w => w.ProductWarehouses.Any(pw => pw.Quantity < pw.Product.LowStockThreshold)).CountAsync();

            var data = await _context.Warehouses.Where(w => w.ProductWarehouses.Any(pw => pw.Quantity < pw.Product.LowStockThreshold))
                .Include(x => x.ProductWarehouses)
                .ThenInclude(x => x.Product)
                .Skip((page - 1) * _pageSize).Take(_pageSize)
                .ToListAsync();

            return new QueryResult<Warehouse>(data, count);
        }

        public async Task<List<Warehouse>> GetWarehousesWithLowStockProducts()
        {
            return await _context.Warehouses.Where(w => w.ProductWarehouses.Any(pw => pw.Quantity < pw.Product.LowStockThreshold))
                .Include(x => x.ProductWarehouses)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }
    }
}
