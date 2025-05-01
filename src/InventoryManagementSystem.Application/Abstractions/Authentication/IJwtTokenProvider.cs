using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Authentication
{
    public interface IJwtTokenProvider
    {
        JwtToken GenerateJwtToken(User user);

        Task<Result<JwtToken>> RefreshAccessToken(Guid refreshToken);

        Task Revoke(long UserId);
    }
}
