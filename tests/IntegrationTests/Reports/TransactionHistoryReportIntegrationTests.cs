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
    public class TransactionHistoryReportIntegrationTests : BaseTest
    {
        public TransactionHistoryReportIntegrationTests(AppFactory factory) : base(factory)
        {

        }

        [Fact]
        public async Task Authorized_ShouldReturnLowStockItems()
        {
            // Arrange
            var accessToken = _jwtTokenProvider.GenerateJwtToken(new User { Email = "test@test.com", Role = Role.Admin });
            var fromDate = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd");
            var toDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            // Act
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            var url = $"reports/transactionHistory?From={fromDate}&To={toDate}&TransactionType=0&page=1";
            var response = await _httpClient.GetAsync(url);
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
            var response = await _httpClient.GetAsync("reports/transactionHistory?page=1");
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
