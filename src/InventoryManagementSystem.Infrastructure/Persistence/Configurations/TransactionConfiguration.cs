using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TransactionDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasConversion(i => (int)i, o => (TransactionType)o);

            builder.Property(t => t.Quantity)
                .IsRequired();

            builder.Property(t => t.ProductId)
                .IsRequired();

            // Shadow property for soft delete
            builder.Property<bool>("IsArchived")
                .HasColumnType("bit")
                .HasDefaultValue(false);

            // Configure soft delete filter
            builder.HasQueryFilter(t => EF.Property<bool>(t, "IsArchived") == false);

            // Configure Constraints
            builder.ToTable("Transactions", t =>
            {
                t.HasCheckConstraint("CK_Transaction_Quantity", "[Quantity] > 0");
                t.HasCheckConstraint("CK_Transaction_TransactionType", "[TransactionType] IN (0, 1, 2)");

            });

            // Configure indexes
            builder.HasIndex(t => t.TransactionDate)
                .HasDatabaseName("IX_Transaction_TransactionDate");


            // Configure relationships
            builder.HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId);

            builder.HasOne(t => t.User)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.UserId);

            builder.HasOne(t => t.Warehouse)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WarehouseId);
        }
    }
}
