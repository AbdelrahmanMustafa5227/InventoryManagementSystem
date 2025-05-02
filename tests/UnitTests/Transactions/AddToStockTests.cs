using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Transactions.Commands;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Transactions
{
    public class AddToStockTests
    {
        private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IWarehouseRepository _warehouseRepository = Substitute.For<IWarehouseRepository>();
        private readonly IProductWarehouseRepository _productWarehouseRepository = Substitute.For<IProductWarehouseRepository>();
        private readonly IAppLogger<AddToStockCommandHandler> _logger = Substitute.For<IAppLogger<AddToStockCommandHandler>>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IUserContext _userContext = Substitute.For<IUserContext>();
        private readonly AddToStockCommandHandler _sut;

        public AddToStockTests()
        {
            _sut = new AddToStockCommandHandler(
                _transactionRepository,
                _productRepository,
                _warehouseRepository,
                _unitOfWork,
                _productWarehouseRepository,
                _userContext,
                _logger);
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenProducteDoesNotExist()
        {
            // Arrange
            var command = new AddToStockCommand(1, 1, 10);
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
            var command = new AddToStockCommand(1, 1, 10);
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(false);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task ShouldReturnSuccess_WhenBothExist()
        {
            // Arrange
            var command = new AddToStockCommand(1, 1, 10);
            _productRepository.Exist(command.ProductId).Returns(true);
            _warehouseRepository.Exist(command.WarehouseId).Returns(true);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            _transactionRepository.Received(1).Add(Arg.Any<Transaction>());
            await _productWarehouseRepository.Received(1).AddToStock(command.ProductId, command.WarehouseId, command.Quantity);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
