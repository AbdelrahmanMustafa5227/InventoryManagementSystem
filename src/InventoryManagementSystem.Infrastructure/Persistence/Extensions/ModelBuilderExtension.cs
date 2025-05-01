using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product A", Description = "Description for Product A", Price = 10.00m, LowStockThreshold = 5 },
                new Product { Id = 2, Name = "Product B", Description = "Description for Product B", Price = 20.00m, LowStockThreshold = 2 },
                new Product { Id = 3, Name = "Product C", Description = "Description for Product C", Price = 30.00m, LowStockThreshold = 3 },
                new Product { Id = 4, Name = "Product D", Description = "Description for Product D", Price = 40.00m, LowStockThreshold = 4 },
                new Product { Id = 5, Name = "Product E", Description = "Description for Product E", Price = 50.00m, LowStockThreshold = 1 },
                new Product { Id = 6, Name = "Product F", Description = "Description for Product F", Price = 60.00m, LowStockThreshold = 5 },
                new Product { Id = 7, Name = "Product G", Description = "Description for Product G", Price = 70.00m, LowStockThreshold = 10 },
                new Product { Id = 8, Name = "Product H", Description = "Description for Product H", Price = 80.00m, LowStockThreshold = 9 },
                new Product { Id = 9, Name = "Product I", Description = "Description for Product I", Price = 90.00m, LowStockThreshold = 11 },
                new Product { Id = 10, Name = "Product J", Description = "Description for Product J", Price = 100.00m, LowStockThreshold = 3 }
                );

            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Warehouse A" },
                new Warehouse { Id = 2, Name = "Warehouse B" },
                new Warehouse { Id = 3, Name = "Warehouse C" }
                );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Admin1", Email = "email1@gmail.com", Password = "Admin123$", Role = Role.Admin },
                new User { Id = 2, Username = "User1", Email = "email2@gmail.com", Password = "User123$", Role = Role.User }
                );

            modelBuilder.Entity<ProductWarehouse>().HasData(
                new ProductWarehouse { ProductId = 1, WarehouseId = 1, Quantity = 10 },
                new ProductWarehouse { ProductId = 2, WarehouseId = 1, Quantity = 20 },
                new ProductWarehouse { ProductId = 3, WarehouseId = 2, Quantity = 30 },
                new ProductWarehouse { ProductId = 4, WarehouseId = 2, Quantity = 40 },
                new ProductWarehouse { ProductId = 5, WarehouseId = 3, Quantity = 50 }
                );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { Id = 1, ProductId = 1, WarehouseId = 1, Quantity = 5, TransactionType = TransactionType.Add, UserId = 1, TransactionDate = new DateTime(2025,3,3) },
                new Transaction { Id = 2, ProductId = 2, WarehouseId = 1, Quantity = 10, TransactionType = TransactionType.Remove, UserId = 2, TransactionDate = new DateTime(2023, 3, 3) },
                new Transaction { Id = 3, ProductId = 3, WarehouseId = 2, Quantity = 15, TransactionType = TransactionType.Add, UserId = 1, TransactionDate = new DateTime(2025, 4, 3) },
                new Transaction { Id = 4, ProductId = 4, WarehouseId = 2, Quantity = 20, TransactionType = TransactionType.Remove, UserId = 2, TransactionDate = new DateTime(2025, 2, 15) },
                new Transaction { Id = 5, ProductId = 5, WarehouseId = 3, Quantity = 25, TransactionType = TransactionType.Add, UserId = 1, TransactionDate = new DateTime(2025, 1, 25) }
                );
        }
    }
}
