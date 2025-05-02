using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Reports.Queries;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Reports
{
    public class TransactionHistoryReportTests
    {
        private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
        private readonly IAppLogger<GetTransactionHistoryQueryHandler> _logger = Substitute.For<IAppLogger<GetTransactionHistoryQueryHandler>>();
        private readonly IUserContext _userContext = Substitute.For<IUserContext>();
        private readonly GetTransactionHistoryQueryHandler _sut;

        public TransactionHistoryReportTests()
        {
            _sut = new GetTransactionHistoryQueryHandler(_transactionRepository, _logger, _userContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnTransactionHistoryReport()
        {
            // Arrange
            var query = new GetTransactionHistoryQuery(DateTime.Now.AddDays(-7), DateTime.Now, TransactionType.Add, 1);
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    Product = new Product { Name = "Product 1" },
                    Quantity = 10,
                    TransactionDate = DateTime.Now.AddDays(-1),
                    TransactionType = TransactionType.Add
                },
                new Transaction
                {
                    Id = 2,
                    Product = new Product { Name = "Product 2" },
                    Quantity = 5,
                    TransactionDate = DateTime.Now.AddDays(-2),
                    TransactionType = TransactionType.Add
                }
            };
            var paginatedTransactions = Paginated<Transaction>.Create(transactions, transactions.Count, 1);
            var queryResult = new QueryResult<Transaction>(transactions, transactions.Count);
            _transactionRepository.GetTransactionHistory(query.From, query.To, query.TransactionType, query.page).Returns(queryResult);
            // Act
            var result = await _sut.Handle(query, CancellationToken.None);
            // Assert
            _logger.Received(1).LogInformation("User {UserId} generated transaction history report successfully", _userContext.GetLoggedUserEmail);
            Assert.NotNull(result);
            Assert.Equal(paginatedTransactions.Items.Count, result.Value.TotalCount);
        }
    }
}
