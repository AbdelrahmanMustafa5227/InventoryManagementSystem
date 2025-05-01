using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedCheckConstraintForProductQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductWarehouse_Quantity",
                table: "ProductWarehouse");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductWarehouse_Quantity",
                table: "ProductWarehouse",
                sql: "Quantity >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductWarehouse_Quantity",
                table: "ProductWarehouse");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductWarehouse_Quantity",
                table: "ProductWarehouse",
                sql: "Quantity > 0");
        }
    }
}
