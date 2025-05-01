using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        Task<QueryResult<Transaction>> GetTransactionHistory(DateTime from, DateTime to, TransactionType transactionType, int page);
        void ArchiveTransactionsOlderThan1Year();
    }
}
