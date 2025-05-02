using InventoryManagementSystem.Application.Features.Reports.Queries;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Products
{
    public class GetProductByIdIntegrationTest : BaseTest
    {
        public GetProductByIdIntegrationTest(AppFactory factory) : base(factory)
        {
        }


        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            // Act
            var response = await _httpClient.GetAsync($"products/details?id={productId}");
            // Assert
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal(productId, product.Id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductNotExists()
        {
            // Arrange
            var productId = 100;
            // Act
            var response = await _httpClient.GetAsync($"products/details?id={productId}");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound , response.StatusCode);
        }

    }
}