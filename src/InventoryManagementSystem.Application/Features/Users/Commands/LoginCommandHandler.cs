using InventoryManagementSystem.Application.Abstractions.Authentication;
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
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(IJwtTokenProvider jwtTokenProvider, IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<JwtToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result.Failure<JwtToken>(Error.UnAuthorized);

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return Result.Failure<JwtToken>(Error.UnAuthorized);

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
