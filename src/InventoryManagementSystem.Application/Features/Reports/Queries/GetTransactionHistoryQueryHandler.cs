using InventoryManagementSystem.Application.Abstractions.Caching;
using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Services;
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
        private readonly IAppLogger<GetTransactionHistoryQueryHandler> _logger;
        private readonly IUserContext _userContext;

        public GetTransactionHistoryQueryHandler(ITransactionRepository transactionRepository, IAppLogger<GetTransactionHistoryQueryHandler> logger, IUserContext userContext)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<Result<Paginated<TransactionDto>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var queryResult = await _transactionRepository.GetTransactionHistory(request.From, request.To, request.TransactionType , request.page);
            var transactionsDto = queryResult.Items
                .Select(t => new TransactionDto(t.Id, t.Product.Name, t.Quantity, t.TransactionDate , t.TransactionType))
                .ToList();

            _logger.LogInformation("User {UserId} generated transaction history report successfully", _userContext.GetLoggedUserEmail);
            return Paginated<TransactionDto>.Create(transactionsDto , request.page , queryResult.TotalRecords);
        }
    }

    public class GetTransactionHistoryQueryValidator : AbstractValidator<GetTransactionHistoryQuery>
    {
        public GetTransactionHistoryQueryValidator()
        {
            RuleFor(x => x.page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("From date is required.");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("To date is required.")
                .GreaterThan(x => x.From)
                .WithMessage("To date must be greater than From date.");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage("Invalid transaction type.");
        }
    }


}
