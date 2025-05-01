using InventoryManagementSystem.Application.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Features.Users.Commands
{
    public record RefreshCommand(Guid RefreshToken) : IRequest<Result<JwtToken>>;

    internal class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<JwtToken>>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public RefreshCommandHandler(IJwtTokenProvider jwtTokenProvider)
        {
            _jwtTokenProvider = jwtTokenProvider;
        }
        public async Task<Result<JwtToken>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            return await _jwtTokenProvider.RefreshAccessToken(request.RefreshToken);
        }
    }
}
