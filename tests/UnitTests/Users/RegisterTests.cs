using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Repositories;
using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Application.Features.Users.Commands;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.UnitTests.Users
{
    public class RegisterTests
    {
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
        private readonly IAppLogger<RegisterCommandHandler> _logger = Substitute.For<IAppLogger<RegisterCommandHandler>>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly RegisterCommandHandler _sut;

        public RegisterTests()
        {
            _sut = new RegisterCommandHandler(_userRepository, _passwordHasher, _unitOfWork, _logger);
        }

        [Fact]
        public async Task ShouldFail_WhenUsernameOrEmailIsUsed()
        {
            // Arrange
            var command = new RegisterCommand("userName", "Pass", "asd@gg.com", Role.User);
            _userRepository.Exist(command.Username, command.Email).Returns(true);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.Conflict, result.Error);
        }

        [Fact]
        public async Task ShouldSucceed_WhenUserOrEmailIsNotUsed()
        {
            // Arrange
            var command = new RegisterCommand("userName", "Pass", "asd@gg.com", Role.User);
            _userRepository.Exist(command.Username, command.Email).Returns(false);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _passwordHasher.Received(1).Hash(command.Password);
            _userRepository.Received(1).Register(Arg.Any<User>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }


    }
}
