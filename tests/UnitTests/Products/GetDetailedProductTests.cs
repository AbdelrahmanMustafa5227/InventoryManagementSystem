using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Features.Products.Queries;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Products
{
    public class GetDetailedProductTests
    {
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly GetDetailedProductQueryHandler _sut;

        public GetDetailedProductTests()
        {
            _sut = new GetDetailedProductQueryHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var productId = 1;
            var request = new GetDetailedProductQuery(productId);
            _productRepository.GetDetailedByIdAsync(request.Id).Returns((Product?)null);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            await _productRepository.Received(1).GetDetailedByIdAsync(request.Id);
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.NotFound, result.Error);
        }

        [Fact]
        public async Task Handle_ProductFound_ReturnsDetailedProductResponse()
        {
            // Arrange
            var productId = 1;
            var request = new GetDetailedProductQuery(productId);
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                LowStockThreshold = 5,
                Transactions = [new Transaction { Id = 1, TransactionType = TransactionType.Add, Quantity = 10 }],
                ProductWarehouse = [new ProductWarehouse { ProductId = 1, Quantity = 20 }]
            };
            _productRepository.GetDetailedByIdAsync(request.Id).Returns(product);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            await _productRepository.Received(1).GetDetailedByIdAsync(request.Id);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
            Assert.Equal("Test Product", result.Value.Name);
            Assert.Equal("Test Description", result.Value.Description);
            Assert.Equal(10.0m, result.Value.Price);
            Assert.Equal(5, result.Value.LowStockThreshold);
            Assert.Single(result.Value.Transactions);
            Assert.Single(result.Value.Warehouses);
        }


    }
}
