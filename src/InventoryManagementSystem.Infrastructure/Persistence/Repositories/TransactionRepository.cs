using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories
{
    internal class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly int _pageSize;

        public TransactionRepository(AppDbContext context , IOptions<PaginationOptions> options)
        {
            _context = context;
            _pageSize = options.Value.PageSize;
        }

        public void Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public async Task<QueryResult<Transaction>> GetTransactionHistory(DateTime from, DateTime to, TransactionType transactionType , int page)
        {
            var totalCount = await _context.Transactions
                .Where(x => x.TransactionDate >= from && x.TransactionDate <= to && x.TransactionType == transactionType)
                .CountAsync();

            var data = await _context.Transactions
                .Where(x => x.TransactionDate >= from && x.TransactionDate <= to && x.TransactionType == transactionType)
                .Include(x => x.Product)
                .Skip((page - 1) * _pageSize).Take(_pageSize)
                .ToListAsync();

            return new QueryResult<Transaction>(data,totalCount);
        }

        public void ArchiveTransactionsOlderThan1Year()
        {
            _context.Database.ExecuteSqlRaw("""
                    UPDATE Transactions
                    SET IsArchived = 1
                    WHERE TransactionDate <= DATEADD(year, -1, GETDATE())
                """);
        }
    }
}
