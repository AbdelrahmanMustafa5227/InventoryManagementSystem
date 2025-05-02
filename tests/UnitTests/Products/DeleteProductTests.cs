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
    public class DeleteProductTests
    {
        private readonly IUnitOfWork _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IAppLogger<DeleteProductCommandHandler> _mockLogger = Substitute.For<IAppLogger<DeleteProductCommandHandler>>();
        private readonly IProductRepository _mockProductRepository = Substitute.For<IProductRepository>();
        private readonly DeleteProductCommandHandler _sut;

        public DeleteProductTests()
        {
            _sut = new DeleteProductCommandHandler(_mockProductRepository, _mockUnitOfWork, _mockLogger);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            var product = new Product { Id = 1, Name = "Test Product", Description = "Test Description" };
            _mockProductRepository.GetByIdAsync(command.Id).Returns(product);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            _mockProductRepository.Received(1).Delete(product);
            await _mockUnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            _mockLogger.Received(1).LogInformation("Product with id {Id} is deleted", command.Id);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            _mockProductRepository.GetByIdAsync(command.Id).Returns((Product?)null);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            _mockLogger.Received(1).LogWarning("Product with id {Id} not found", command.Id);
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }
    }
}
