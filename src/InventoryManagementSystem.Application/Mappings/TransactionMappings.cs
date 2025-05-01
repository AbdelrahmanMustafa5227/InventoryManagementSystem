using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Transactions.Commands;
using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Mappings
{
    public static class TransactionMappings
    {
        public static Transaction ToModel(this AddToStockCommand command)
        {
            return new Transaction
            {
                WarehouseId = command.WarehouseId,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                TransactionType = TransactionType.Add,
                TransactionDate = DateTime.UtcNow
            };
        }

        public static Transaction ToModel(this RemoveFromStockCommand command)
        {
            return new Transaction
            {
                WarehouseId = command.WarehouseId,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                TransactionType = TransactionType.Remove,
                TransactionDate = DateTime.UtcNow
            };
        }

        public static Transaction ToModel(this TransferCommand command)
        {
            return new Transaction
            {
                WarehouseId = command.SourceWarehouseId,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                TransactionType = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow
            };
        }
    }
}
