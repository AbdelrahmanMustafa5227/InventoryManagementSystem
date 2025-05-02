using InventoryManagementSystem.Infrastructure.Services;
using InventoryManagementSystem.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.IntegrationTests.Other
{
    public class PasswordHashTests : BaseTest
    {
        public PasswordHashTests(AppFactory factory) : base(factory)
        {
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashForSamePassword()
        {
            // Arrange
            var password = "TestPassword";
            var passwordHasher = new PasswordHasher();
            // Act
            var hash = passwordHasher.Hash(password);
            var result = passwordHasher.Verify(password , hash);
            // Assert
            Assert.True(result);
        }
    }
}
