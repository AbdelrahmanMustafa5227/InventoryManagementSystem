using InventoryManagementSystem.Application.Abstractions.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InventoryManagementSystem.Api.Helpers
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetLoggedUserId => long.TryParse(
            _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            , out var Id) ? Id : -1;

        public string GetLoggedUserEmail => 
            _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value!;
    }
}
