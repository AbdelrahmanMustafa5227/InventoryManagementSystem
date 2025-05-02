using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Features.Users.Commands;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Users
{
    public class LoginIntegrationTests : BaseTest
    {
        public LoginIntegrationTests(AppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ValidCredentials_ShouldReturnJwtToken()
        {
            // Arrange
            var command = new LoginCommand("test@test.com", "Admin123$");
            // Act
            var response = await _httpClient.PostAsJsonAsync("auth/login", command);
            var content = await response.Content.ReadFromJsonAsync<JwtToken>();
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }
    }
}
