using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Abstractions.Logging;
using InventoryManagementSystem.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Users.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<JwtToken>>;

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, Result<JwtToken>>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAppLogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IJwtTokenProvider jwtTokenProvider, IUserRepository userRepository, IPasswordHasher passwordHasher, IAppLogger<LoginCommandHandler> logger)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<JwtToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("Login failed for user {0}", request.Email);
                return Result.Failure<JwtToken>(Error.UnAuthorized);
            }

            _logger.LogInformation("User {0} logged in successfully", request.Email);
            return _jwtTokenProvider.GenerateJwtToken(user);
        }

        public class LoginCommandValidator : AbstractValidator<LoginCommand>
        {
            public LoginCommandValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(100);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                    .MaximumLength(100);
            }
        }
    }
}
