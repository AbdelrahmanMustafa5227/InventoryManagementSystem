using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using InventoryManagementSystem.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Transactions.Commands
{
    public record RemoveFromStockCommand(long ProductId, long WarehouseId, int Quantity) : IRequest<Result>;

    internal class RemoveFromStockCommandHandler : IRequestHandler<RemoveFromStockCommand, Result>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IPublisher _publisher;

        public RemoveFromStockCommandHandler(ITransactionRepository transactionRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IProductWarehouseRepository productWarehouseRepository, IPublisher publisher, IUserContext userContext)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _productWarehouseRepository = productWarehouseRepository;
            _publisher = publisher;
            _userContext = userContext;
        }

        public async Task<Result> Handle(RemoveFromStockCommand request, CancellationToken cancellationToken)
        {
            if (!await _productRepository.Exist(request.ProductId)
                || !await _warehouseRepository.Exist(request.WarehouseId))
                return Result.Failure(Error.NotFound);

            var productWarehouse = await _productWarehouseRepository.GetDetailedByIdAsync(request.ProductId, request.WarehouseId);

            if (productWarehouse is null)
                return Result.Failure(Error.NotFound);
            if (productWarehouse.Quantity < request.Quantity)
                return Result.Failure(new Error("Warehouse Has Quantity less than requested"));

            var transaction = request.ToModel();
            transaction.UserId = _userContext.GetLoggedUserId;
            _transactionRepository.Add(transaction);
            await _productWarehouseRepository.RemoveFromStock(request.ProductId, request.WarehouseId, request.Quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (productWarehouse.Product.LowStockThreshold >= productWarehouse.Quantity)
                await _publisher.Publish(new LowStockEvent(productWarehouse.Product.Name, productWarehouse.Warehouse.Name));

            return Result.Success();
        }
    }

    public class RemoveFromStockCommandValidator : AbstractValidator<RemoveFromStockCommand>
    {
        public RemoveFromStockCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
