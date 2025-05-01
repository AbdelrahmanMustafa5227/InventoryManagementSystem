using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(long id);

        Task<Product?> GetDetailedByIdAsync(long id);

        Task<QueryResult<Product>> GetAllAsync(int page);

        Task<bool> Exist(long productId);

        void Add(Product product);

        void Update(Product product);

        void Delete(Product product);
    }
}
