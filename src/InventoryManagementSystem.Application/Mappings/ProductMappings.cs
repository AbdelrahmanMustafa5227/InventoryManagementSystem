using InventoryManagementSystem.Application.Features.Products.Commands;
using InventoryManagementSystem.Application.Features.Products.Queries;
using InventoryManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Mappings
{
    internal static class ProductMappings
    {
        public static Product ToModel(this AddProductCommand command)
        {
            return new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                LowStockThreshold = command.LowStockThreshold
            };
        }

        public static void ApplyChanges(this Product product, UpdateProductCommand command)
        {
            product.Id = command.Id;
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.LowStockThreshold = command.LowStockThreshold;
        }

        public static AddProductResponse ToResponse(this Product product)
        {
            return new AddProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.LowStockThreshold
            );
        }

        public static GetDetailedProductResponse ToDetailedResponse(this Product product)
        {
            return new GetDetailedProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.LowStockThreshold,
                product.Transactions.Select(t => new TransactionDTO(t.Id , t.TransactionType , t.Quantity)).ToList(),
                product.ProductWarehouse.Select(x => new WarehouseDTO(x.WarehouseId, x.Quantity)).ToList()
            );
        }

        public static List<GetAllProductsResponse> ToResponse(this IEnumerable<Product> products)
        {
            return products.Select(p => new GetAllProductsResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.LowStockThreshold
            )).ToList();
        }

    }
}
