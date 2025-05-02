using InventoryManagementSystem.Application.Features.Products.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.ValidatorsTests
{
    public class ValidationTests
    {
        private readonly AddProductCommandValidator _addProductCommandValidator = new AddProductCommandValidator();

        [Theory]
        [InlineData("", "Valid Description", 10, 5, false)]
        [InlineData("Valid Name", "Valid Description", -10, 5, false)]
        [InlineData("Valid Name", "Valid Description", 10, -5, false)]
        [InlineData("Valid Name", "Valid Description", 10, 0, true)]
        public async Task AddProductCommandValidator_ValidInput_ShouldNotHaveValidationError(string name, string description, decimal price, int threshold , bool isValid)
        {
            // Arrange
            var command = new AddProductCommand(name, description, price, threshold);
            // Act
            var result = await _addProductCommandValidator.ValidateAsync(command);
            // Assert
            Assert.Equal(result.IsValid , isValid);
        }

    }
}
