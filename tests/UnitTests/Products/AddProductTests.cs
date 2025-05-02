using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Exceptions;
using InventoryManagementSystem.Application.Features.Products.Commands;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace InventoryManagementSystem.UnitTests.Products
{
    public class AddProductTests
    {
        private readonly IUnitOfWork _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IAppLogger<AddProductCommandHandler> _mockLogger = Substitute.For<IAppLogger<AddProductCommandHandler>>();
        private readonly IProductRepository _mockProductRepository = Substitute.For<IProductRepository>();
        private readonly AddProductCommandHandler _sut;

        public AddProductTests()
        {
            _sut = new AddProductCommandHandler(_mockProductRepository, _mockUnitOfWork , _mockLogger);
        }

        [Fact]
        public async Task Handle_ShouldAddProduct_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddProductCommand("Test Product", "Test Description", 10.0m, 5);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            _mockProductRepository.Received(1).Add(Arg.Is<Product>(p => p.Name == command.Name && p.Description == command.Description));
            await _mockUnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            _mockLogger.Received(1).LogInformation("Product with Id {ProductId} Has Successfully been created", Arg.Any<long>());
            Assert.True(result.IsSuccess);
        }

    }
}
