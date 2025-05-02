using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Users.Commands;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Users
{
    public class LoginTests
    {
        private readonly IJwtTokenProvider _jwtTokenProvider = Substitute.For<IJwtTokenProvider>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
        private readonly IAppLogger<LoginCommandHandler> _logger = Substitute.For<IAppLogger<LoginCommandHandler>>();
        private readonly LoginCommandHandler _sut;

        public LoginTests()
        {
            _sut = new LoginCommandHandler(_jwtTokenProvider, _userRepository, _passwordHasher, _logger);
        }

        [Fact]
        public async Task ShouldFail_WhenEmailNotFound()
        {
            // Arrange
            var command = new LoginCommand("Email", "Password");
            _userRepository.GetByEmailAsync(command.Email).Returns((User?)null);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.UnAuthorized, result.Error);
        }

        [Fact]
        public async Task ShouldFail_WhenPasswordIsWrong()
        {
            // Arrange
            var command = new LoginCommand("Email", "WrongPassword");
            var user = new User { Email = command.Email, Password = "Password" };
            _userRepository.GetByEmailAsync(command.Email).Returns(user);
            _passwordHasher.Verify(command.Password, user.Password).Returns(false);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.UnAuthorized, result.Error);
        }

        [Fact]
        public async Task ShouldReturnToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var command = new LoginCommand("Email", "Password");
            var user = new User { Email = command.Email, Password = "Password" };
            _userRepository.GetByEmailAsync(command.Email).Returns(user);
            _passwordHasher.Verify(command.Password, user.Password).Returns(true);
            var token = new JwtToken("accessToken", Guid.NewGuid());
            _jwtTokenProvider.GenerateJwtToken(user).Returns(token);
            // Act
            var result = await _sut.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(token, result.Value);
        }
    }
}
