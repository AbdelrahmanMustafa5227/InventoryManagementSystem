using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Features.Products.Commands;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Products
{
    public class UpdateProductTests
    {
        private readonly IUnitOfWork _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IAppLogger<UpdateProductCommandHandler> _mockLogger = Substitute.For<IAppLogger<UpdateProductCommandHandler>>();
        private readonly IProductRepository _mockProductRepository = Substitute.For<IProductRepository>();
        private readonly UpdateProductCommandHandler _sut;

        public UpdateProductTests()
        {
            _sut = new UpdateProductCommandHandler(_mockProductRepository, _mockUnitOfWork, _mockLogger);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new UpdateProductCommand(1, "Test Product", "Test Description", 10.0m, 5);
            _mockProductRepository.GetByIdAsync(command.Id).Returns((Product?)null);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            await _mockProductRepository.Received(1).GetByIdAsync(command.Id);
            await _mockUnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
            _mockLogger.Received(1).LogError("Product with ID {Id} not found", command.Id);
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task Handle_ProductFound_UpdatesProductAndReturnsSuccess()
        {
            // Arrange
            var command = new UpdateProductCommand(1, "Test Product", "Test Description", 10.0m, 5);
            var product = new Product { Id = command.Id, Name = "Old Name", Description = "Old Description", Price = 5.0m, LowStockThreshold = 2 };
            _mockProductRepository.GetByIdAsync(command.Id).Returns(product);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            await _mockProductRepository.Received(1).GetByIdAsync(command.Id);
            await _mockUnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            _mockLogger.Received(1).LogInformation("Product with ID {Id} updated successfully", command.Id);
            Assert.True(result.IsSuccess);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal("Test Description", product.Description);
            Assert.Equal(10.0m, product.Price);
            Assert.Equal(5, product.LowStockThreshold);
        }
    }
}
