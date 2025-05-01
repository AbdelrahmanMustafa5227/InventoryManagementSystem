using InventoryManagementSystem.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Transactions.Commands
{
    public record AddToStockCommand(long ProductId, long WarehouseId, int Quantity) : IRequest<Result>;

    internal class AddToStockCommandHandler : IRequestHandler<AddToStockCommand, Result>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public AddToStockCommandHandler(ITransactionRepository transactionRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork, IProductWarehouseRepository productWarehouseRepository, IUserContext userContext)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
            _productWarehouseRepository = productWarehouseRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(AddToStockCommand request, CancellationToken cancellationToken)
        {
            if (!await _productRepository.Exist(request.ProductId)
                || !await _warehouseRepository.Exist(request.WarehouseId))
                return Result.Failure(Error.NotFound);

            var transaction = request.ToModel();
            transaction.UserId = _userContext.GetLoggedUserId;
            _transactionRepository.Add(transaction);
            await _productWarehouseRepository.AddToStock(request.ProductId, request.WarehouseId, request.Quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    public class AddToStockCommandValidator : AbstractValidator<AddToStockCommand>
    {
        public AddToStockCommandValidator()
        {
            RuleFor(x => x.ProductId).NotNull().WithMessage("Product ID is required.");
            RuleFor(x => x.WarehouseId).NotNull().WithMessage("Warehouse ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
