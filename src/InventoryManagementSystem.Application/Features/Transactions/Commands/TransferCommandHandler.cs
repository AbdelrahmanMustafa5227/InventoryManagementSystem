using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.EmailTemplates;
using InventoryManagementSystem.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Transactions.Commands
{
    public record TransferCommand(long SourceWarehouseId, long DestinationWarehouseId, long ProductId, int Quantity) : IRequest<Result>;

    internal class TransferCommandHandler : IRequestHandler<TransferCommand, Result>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;
        private readonly IAppLogger<TransferCommandHandler> _logger;

        public TransferCommandHandler(ITransactionRepository transactionRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork, IProductWarehouseRepository productWarehouseRepository, IUserContext userContext, IPublisher publisher, IAppLogger<TransferCommandHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
            _productWarehouseRepository = productWarehouseRepository;
            _userContext = userContext;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<Result> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            if (!await _productRepository.Exist(request.ProductId)
                || !await _warehouseRepository.Exist(request.SourceWarehouseId)
                || !await _warehouseRepository.Exist(request.DestinationWarehouseId))
                return Result.Failure(Error.NotFound);

            var productWarehouse = await _productWarehouseRepository.GetDetailedByIdAsync(request.ProductId, request.SourceWarehouseId);
            if (productWarehouse is null)
                return Result.Failure(Error.NotFound);
            if (productWarehouse.Quantity < request.Quantity)
                return Result.Failure(new Error("Warehouse Has Quantity less than requested"));

            var transaction = request.ToModel();
            transaction.UserId = _userContext.GetLoggedUserId;
            _transactionRepository.Add(transaction);

            await _productWarehouseRepository.TransferStock(request.ProductId, request.SourceWarehouseId, request.DestinationWarehouseId, request.Quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (productWarehouse.Product.LowStockThreshold >= productWarehouse.Quantity)
                await _publisher.Publish(new LowStockEvent(productWarehouse.Product.Name, productWarehouse.Warehouse.Name));

            _logger.LogInformation("User {UserId} transferred {Quantity} units of product {ProductId} from warehouse {SourceWarehouseId} to warehouse {DestinationWarehouseId}", _userContext.GetLoggedUserEmail, request.Quantity, request.ProductId, request.SourceWarehouseId, request.DestinationWarehouseId);
            return Result.Success();
        }
    }

    public class TransferCommandValidator : AbstractValidator<TransferCommand>
    {
        public TransferCommandValidator()
        {
            RuleFor(x => x.SourceWarehouseId).NotEmpty();
            RuleFor(x => x.DestinationWarehouseId).NotEmpty()
                .Must((dto, x) => dto.SourceWarehouseId != x).WithMessage("You Transfer To the same warehouse");
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
