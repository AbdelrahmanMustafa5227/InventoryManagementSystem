using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Abstractions.Authentication
{
    public class JwtToken
    {
        public string AccessToken { get; }
        public Guid RefreshToken { get; }
        public JwtToken(string accessToken, Guid refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
