using InventoryManagementSystem.Application.Abstractions.Services;
using InventoryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Users.Commands
{
    public record RegisterCommand(string Username, string Password, string Email, Role Role) : IRequest<Result>;


    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.Exist(request.Username, request.Email))
                return Result.Failure(Error.Conflict);

            var user = request.ToModel();
            user.Password = _passwordHasher.Hash(request.Password);

            _userRepository.Register(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
        {
            public RegisterCommandValidator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
                    .MaximumLength(100);

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .MaximumLength(100)
                    .EmailAddress();

                RuleFor(x => x.Role)
                    .NotNull()
                    .IsInEnum();
            }
        }
    }
}
