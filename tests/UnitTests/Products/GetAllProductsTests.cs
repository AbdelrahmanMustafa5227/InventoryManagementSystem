using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Features.Products.Queries;
using InventoryManagementSystem.Application.Helpers.Pagination;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Products
{
    public class GetAllProductsTests
    {
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly GetAllProductsQueryHandler _sut;

        public GetAllProductsTests()
        {
            _sut = new GetAllProductsQueryHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginatedProducts()
        {
            // Arrange
            var pageNumber = 1;
            var request = new GetAllProductsQuery(pageNumber);
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.0m, LowStockThreshold = 5 },
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.0m, LowStockThreshold = 10 }
            };
            var paginatedResult = new Paginated<Product>(products, pageNumber, products.Count);
            var queryResult = new QueryResult<Product>(products, products.Count);
            _productRepository.GetAllAsync(pageNumber).Returns(queryResult);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(products.Count, paginatedResult.Items.Count());
        }

        [Fact]
        public async Task Handle_InvalidPageNumber_ReturnsError()
        {
            // Arrange
            var pageNumber = 1;
            var request = new GetAllProductsQuery(pageNumber);
            List<Product> products = [];
            var paginatedResult = new Paginated<Product>(products, pageNumber, products.Count);
            var queryResult = new QueryResult<Product>(products, products.Count);
            _productRepository.GetAllAsync(pageNumber).Returns(queryResult);
            // Act
            var result = await _sut.Handle(request, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }
    }
}
