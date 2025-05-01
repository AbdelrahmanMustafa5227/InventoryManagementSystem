using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "LowStockThreshold", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, "Description for Product A", 5, "Product A", 10.00m },
                    { 2L, "Description for Product B", 2, "Product B", 20.00m },
                    { 3L, "Description for Product C", 3, "Product C", 30.00m },
                    { 4L, "Description for Product D", 4, "Product D", 40.00m },
                    { 5L, "Description for Product E", 1, "Product E", 50.00m },
                    { 6L, "Description for Product F", 5, "Product F", 60.00m },
                    { 7L, "Description for Product G", 10, "Product G", 70.00m },
                    { 8L, "Description for Product H", 9, "Product H", 80.00m },
                    { 9L, "Description for Product I", 11, "Product I", 90.00m },
                    { 10L, "Description for Product J", 3, "Product J", 100.00m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1L, "email1@gmail.com", "Admin123$", 1, "Admin1" },
                    { 2L, "email2@gmail.com", "User123$", 0, "User1" }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Warehouse A" },
                    { 2L, "Warehouse B" },
                    { 3L, "Warehouse C" }
                });

            migrationBuilder.InsertData(
                table: "ProductWarehouse",
                columns: new[] { "ProductId", "WarehouseId", "Quantity" },
                values: new object[,]
                {
                    { 1L, 1L, 10 },
                    { 2L, 1L, 20 },
                    { 3L, 2L, 30 },
                    { 4L, 2L, 40 },
                    { 5L, 3L, 50 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "ProductId", "Quantity", "TransactionDate", "TransactionType", "UserId", "WarehouseId" },
                values: new object[,]
                {
                    { 1L, 1L, 5, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1L, 1L },
                    { 2L, 2L, 10, new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2L, 1L },
                    { 3L, 3L, 15, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1L, 2L },
                    { 4L, 4L, 20, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2L, 2L },
                    { 5L, 5L, 25, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1L, 3L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductWarehouse",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "ProductWarehouse",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "ProductWarehouse",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { 3L, 2L });

            migrationBuilder.DeleteData(
                table: "ProductWarehouse",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { 4L, 2L });

            migrationBuilder.DeleteData(
                table: "ProductWarehouse",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { 5L, 3L });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
