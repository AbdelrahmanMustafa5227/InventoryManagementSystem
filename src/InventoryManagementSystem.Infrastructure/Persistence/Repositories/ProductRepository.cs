using InventoryManagementSystem.Application.Abstractions.Caching;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly int _pageSize;
        private readonly ICachingService _cachingService;

        public ProductRepository(AppDbContext context, IOptions<PaginationOptions> options, ICachingService cachingService)
        {
            _context = context;
            _pageSize = options.Value.PageSize;
            _cachingService = cachingService;
        }

        public async Task<Product?> GetByIdAsync(long Id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<Product?> GetDetailedByIdAsync(long id)
        {
            return await _context.Products
                .Include(x => x.Transactions)
                .Include(x => x.ProductWarehouse)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<QueryResult<Product>> GetAllAsync(int page)
        {
            var totalCount = await _context.Products.CountAsync();
            var data = await _context.Products.Skip((page - 1) * _pageSize).Take(_pageSize).ToListAsync();
            return new QueryResult<Product>(data, totalCount);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _cachingService.Remove(x => x.StartsWith("All-Products"));
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> Exist(long productId)
        {
            return await _context.Products.AnyAsync(x => x.Id == productId);
        }
    }
}
