using InventoryManagementSystem.Application.Abstractions.Caching;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Reports.Queries
{
    public record GetTransactionHistoryQuery(DateTime From, DateTime To, TransactionType TransactionType, int page) : IRequest<Result<Paginated<TransactionDto>>>;

    public record TransactionDto(long Id, string ProductName, int Quantity, DateTime TransactionDate , TransactionType Type);

    internal class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, Result<Paginated<TransactionDto>>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionHistoryQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<Paginated<TransactionDto>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var queryResult = await _transactionRepository.GetTransactionHistory(request.From, request.To, request.TransactionType , request.page);
            var transactionsDto = queryResult.Items
                .Select(t => new TransactionDto(t.Id, t.Product.Name, t.Quantity, t.TransactionDate , t.TransactionType))
                .ToList();

            return Paginated<TransactionDto>.Create(transactionsDto , request.page , queryResult.TotalRecords);
        }
    }
    

}
