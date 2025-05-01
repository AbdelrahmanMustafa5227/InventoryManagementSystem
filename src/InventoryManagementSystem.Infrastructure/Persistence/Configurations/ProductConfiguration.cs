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
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.LowStockThreshold)
                .IsRequired();

            //Configure Constriants
            builder.ToTable("Products" , t =>
            {
                t.HasCheckConstraint("CK_Product_Price", "Price > 0");
                t.HasCheckConstraint("CK_Product_LowStockThreshold", "LowStockThreshold >= 0");
            });

            // Configure Indeces
            builder.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name");

            builder.HasIndex(p => p.Price)
                .HasDatabaseName("IX_Product_Price");

        }
    }
}
