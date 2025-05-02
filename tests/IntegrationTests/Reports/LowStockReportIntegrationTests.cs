using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Reports
{
    public class LowStockReportIntegrationTests : BaseTest
    {
        public LowStockReportIntegrationTests(AppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Authorized_ShouldReturnLowStockItems()
        {
            // Arrange
            var accessToken = _jwtTokenProvider.GenerateJwtToken(new User { Email = "test@test.com" , Role = Role.Admin });
            // Act
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            var response = await _httpClient.GetAsync("reports/lowstock?page=1");
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }
        [Fact]
        public async Task Unauthorized_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await _httpClient.GetAsync("reports/lowstock?page=1");
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
    
}
