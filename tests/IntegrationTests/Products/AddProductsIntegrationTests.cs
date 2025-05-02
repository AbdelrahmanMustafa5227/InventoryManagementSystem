using Hangfire.Dashboard;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementSystem.Application.Features.Products.Commands;

namespace InventoryManagementSystem.IntegrationTests.Products
{
    public class AddProductsIntegrationTests : BaseTest
    {
        public AddProductsIntegrationTests(AppFactory factory) : base(factory)
        {

        }

        [Fact]
        public async Task ShouldReturnProduct_WhenValid()
        {
            // Arrange
            var command = new AddProductCommand("Redmi Note 12", "Xiaomi", 2000 , 10);
            // Act
            _httpClient.DefaultRequestHeaders.Add("X-Idempotency", Guid.NewGuid().ToString());
            var response = await _httpClient.PostAsJsonAsync("products/add", command);
            var deserializedContent = await response.Content.ReadFromJsonAsync<AddProductResponse>();
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(command.Name, deserializedContent!.Name);
            Assert.Equal(command.Description, deserializedContent!.Description);
            Assert.Equal(command.Price, deserializedContent!.Price);
        }

        [Fact]
        public async Task ShouldReturnBadRequest_WhenInvalid()
        {
            // Arrange
            var command = new AddProductCommand("", "", -1, -1);
            // Act
            _httpClient.DefaultRequestHeaders.Add("X-Idempotency", Guid.NewGuid().ToString());
            var response = await _httpClient.PostAsJsonAsync("products/add", command);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnBadRequest_WhenNoIdempotencyId()
        {
            // Arrange
            var command = new AddProductCommand("Redmi Note 12", "Xiaomi", 2000, 10);
            // Act
            var response = await _httpClient.PostAsJsonAsync("products/add", command);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
