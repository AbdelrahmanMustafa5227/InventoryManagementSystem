using InventoryManagementSystem.Application.Features.Transactions.Commands;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Transactions
{
    public class AddToStockIntegrationTests : BaseTest
    {
        public AddToStockIntegrationTests(AppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AddToStock_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new AddToStockCommand(1, 1, 10);
            var accessToken = _jwtTokenProvider.GenerateJwtToken(new User { Email = "email1@gmail.com", Role = Role.Admin });
            // Act
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("X-Idempotency", Guid.NewGuid().ToString());
            var response = await _httpClient.PostAsJsonAsync("transactions/addToStock", request);
            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
