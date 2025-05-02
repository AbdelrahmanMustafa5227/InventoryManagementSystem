using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Events;
using InventoryManagementSystem.Application.Features.Transactions.Commands;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using MediatR;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Transactions
{
    public class TransferStockTests
    {
        private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IWarehouseRepository _warehouseRepository = Substitute.For<IWarehouseRepository>();
        private readonly IProductWarehouseRepository _productWarehouseRepository = Substitute.For<IProductWarehouseRepository>();
        private readonly IAppLogger<TransferCommandHandler> _logger = Substitute.For<IAppLogger<TransferCommandHandler>>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IUserContext _userContext = Substitute.For<IUserContext>();
        private readonly IPublisher _publisher = Substitute.For<IPublisher>();
        private readonly TransferCommandHandler _sut;

        public TransferStockTests()
        {
            _sut = new TransferCommandHandler(_transactionRepository,
                _productRepository,
                _warehouseRepository,
                _unitOfWork,
                _productWarehouseRepository,
                _userContext,
                _publisher,
                _logger);
        }

        [Fact]
        public async Task ShouldTransferStockAndNotSendEmail_WhenThresholdNotMet()
        {
            // Arrange
            var request = new TransferCommand(1, 2, 3, 10);
            var productWarehouse = new ProductWarehouse
            {
                Product = new Product { LowStockThreshold = 5 },
                Warehouse = new Warehouse(),
                Quantity = 20
            };
            _productRepository.Exist(request.ProductId).Returns(true);
            _warehouseRepository.Exist(request.SourceWarehouseId).Returns(true);
            _warehouseRepository.Exist(request.DestinationWarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(request.ProductId, request.SourceWarehouseId)
                .Returns(productWarehouse);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            _transactionRepository.Received(1).Add(Arg.Is<Transaction>(t => t.Quantity == request.Quantity));
            await _productWarehouseRepository.Received(1).TransferStock(request.ProductId, request.SourceWarehouseId, request.DestinationWarehouseId, request.Quantity);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _publisher.DidNotReceive().Publish(Arg.Any<LowStockEvent>());
        }

        [Fact]
        public async Task ShouldTransferStockAndSendEmail_WhenThresholdMet()
        {
            // Arrange
            var request = new TransferCommand(1, 2, 3, 10);
            var productWarehouse = new ProductWarehouse
            {
                Product = new Product { LowStockThreshold = 10 },
                Warehouse = new Warehouse(),
                Quantity = 10
            };
            _productRepository.Exist(request.ProductId).Returns(true);
            _warehouseRepository.Exist(request.SourceWarehouseId).Returns(true);
            _warehouseRepository.Exist(request.DestinationWarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(request.ProductId, request.SourceWarehouseId)
                .Returns(productWarehouse);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            _transactionRepository.Received(1).Add(Arg.Is<Transaction>(t => t.Quantity == request.Quantity));
            await _productWarehouseRepository.Received(1).TransferStock(request.ProductId, request.SourceWarehouseId, request.DestinationWarehouseId, request.Quantity);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _publisher.Received(1).Publish(Arg.Is<LowStockEvent>(e => e.ProductName == productWarehouse.Product.Name && e.WarehouseName == productWarehouse.Warehouse.Name));
        }

        [Fact]
        public async Task ShouldReturnNotFound_WhenProductOrWarehouseDoesNotExist()
        {
            // Arrange
            var request = new TransferCommand(1, 2, 3, 10);
            _productRepository.Exist(request.ProductId).Returns(false);
            _warehouseRepository.Exist(request.SourceWarehouseId).Returns(true);
            _warehouseRepository.Exist(request.DestinationWarehouseId).Returns(true);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenQuantityIsNotAvailable()
        {
            // Arrange
            var request = new TransferCommand(1, 2, 3, 10);
            var productWarehouse = new ProductWarehouse
            {
                Product = new Product(),
                Warehouse = new Warehouse(),
                Quantity = 5
            };
            _productRepository.Exist(request.ProductId).Returns(true);
            _warehouseRepository.Exist(request.SourceWarehouseId).Returns(true);
            _warehouseRepository.Exist(request.DestinationWarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(request.ProductId, request.SourceWarehouseId)
                .Returns(productWarehouse);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Warehouse Has Quantity less than requested", result.Error?.Message);
        }
    }
}
