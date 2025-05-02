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
    public class RemoveFromStockTests
    {
        private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IWarehouseRepository _warehouseRepository = Substitute.For<IWarehouseRepository>();
        private readonly IProductWarehouseRepository _productWarehouseRepository = Substitute.For<IProductWarehouseRepository>();
        private readonly IAppLogger<RemoveFromStockCommandHandler> _logger = Substitute.For<IAppLogger<RemoveFromStockCommandHandler>>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IUserContext _userContext = Substitute.For<IUserContext>();
        private readonly IPublisher _publisher = Substitute.For<IPublisher>();
        private readonly RemoveFromStockCommandHandler _sut;

        public RemoveFromStockTests()
        {
            _sut = new RemoveFromStockCommandHandler(_transactionRepository,
                _productRepository,
                _warehouseRepository,
                _unitOfWork,
                _productWarehouseRepository,
                _publisher,
                _userContext,
                _logger);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            _productRepository.Exist(command.ProductId).Returns(false);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenWarehouseDoesNotExist()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(false);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenProductWarehouseDoesNotExist()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(command.ProductId, command.WarehouseId).Returns((ProductWarehouse?)null);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenQuantityIsGreaterThanAvailable()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            var productWarehouse = new ProductWarehouse { Quantity = 5 };
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(command.ProductId, command.WarehouseId).Returns(productWarehouse);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Warehouse Has Quantity less than requested", result.Error!.Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessAndNotSendEmail_WhenAllConditionsAreMetAndThresholdNotMet()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            var productWarehouse = new ProductWarehouse { Quantity = 20, Product = new Product { LowStockThreshold = 5 } };
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(command.ProductId, command.WarehouseId).Returns(productWarehouse);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            _transactionRepository.Received(1).Add(Arg.Any<Transaction>());
            await _productWarehouseRepository.Received(1).RemoveFromStock(command.ProductId, command.WarehouseId, command.Quantity);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _publisher.DidNotReceive().Publish(Arg.Any<LowStockEvent>());
        }

        [Fact]
        public async Task ShouldReturnSuccessAndSendEmail_WhenAllConditionsAreMetAndThresholdMet()
        {
            // Arrange
            var command = new RemoveFromStockCommand(1, 1, 10);
            var productWarehouse = new ProductWarehouse { Quantity = 20, Product = new Product { LowStockThreshold = 15 }, Warehouse = new Warehouse { Name = "W1" } };
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            _productWarehouseRepository.GetDetailedByIdAsync(command.ProductId, command.WarehouseId).Returns(productWarehouse);
            _productWarehouseRepository
                .When(x =>x.RemoveFromStock(command.ProductId, command.WarehouseId, command.Quantity))
                .Do( _ => productWarehouse.Quantity -= command.Quantity);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            _transactionRepository.Received(1).Add(Arg.Any<Transaction>());
            await _productWarehouseRepository.Received(1).RemoveFromStock(command.ProductId, command.WarehouseId, command.Quantity);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _publisher.Received(1).Publish(Arg.Any<LowStockEvent>());
        }

    }
}
