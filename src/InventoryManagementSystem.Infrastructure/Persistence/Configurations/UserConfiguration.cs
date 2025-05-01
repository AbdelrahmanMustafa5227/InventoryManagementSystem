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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion(i => (int)i, o => (Role)o);

            // Configure Constraints
            builder.ToTable("Users", t =>
            {
                t.HasCheckConstraint("CK_User_Role", "Role IN (0, 1)");
                t.HasCheckConstraint("CK_User_Email", "Email LIKE '_%@_%._%'");
                t.HasCheckConstraint("CK_User_Password", "LEN(Password) >= 6");
            });

            // Configure indexes
            builder.HasIndex(u => new { u.Username, u.Password })
                .HasDatabaseName("IX_User_Username_Password");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");
        }
    }

}
