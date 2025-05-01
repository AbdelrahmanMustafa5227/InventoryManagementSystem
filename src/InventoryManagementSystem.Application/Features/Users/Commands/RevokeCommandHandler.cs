using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Users.Commands
{
    public record RevokeCommand() : IRequest<Result>;
   
    internal class RevokeCommandHandler : IRequestHandler<RevokeCommand, Result>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserContext _userContext;

        public RevokeCommandHandler(IJwtTokenProvider jwtTokenProvider, IUserContext userContext)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userContext = userContext;
        }
        public async Task<Result> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            long userId = _userContext.GetLoggedUserId;
            if (userId == -1)
                return Result.Failure(Error.Forbidden);

            await _jwtTokenProvider.Revoke(userId);
            return Result.Success();
        }
    }

}
