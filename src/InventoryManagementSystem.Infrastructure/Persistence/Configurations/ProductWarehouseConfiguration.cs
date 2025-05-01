using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class ProductWarehouseConfiguration : IEntityTypeConfiguration<ProductWarehouse>
    {
        public void Configure(EntityTypeBuilder<ProductWarehouse> builder)
        {
            builder.HasKey(pw => new { pw.ProductId, pw.WarehouseId });

            builder.Property(pw => pw.Quantity)
                .IsRequired();

            // Configure the constraints
            builder.ToTable("ProductWarehouse" , t =>
            {
                t.HasCheckConstraint("CK_ProductWarehouse_Quantity", "Quantity >= 0");
            });

            // Configure the relationships
            builder
                .HasOne(pw => pw.Product)
                .WithMany(p => p.ProductWarehouse)
                .HasForeignKey(pw => pw.ProductId);

            builder
                .HasOne(pw => pw.Warehouse)
                .WithMany(w => w.ProductWarehouses)
                .HasForeignKey(pw => pw.WarehouseId);
        }
    }

}
