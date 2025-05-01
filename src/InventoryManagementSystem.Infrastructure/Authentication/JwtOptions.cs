using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public const string SectionName = "JwtOptions";

        [MinLength(20, ErrorMessage = "Jwt Sercret must be longer than 20 characters")]
        public string SecretKey { get; init; } = string.Empty;

        [NotNull]
        public string Issuer { get; init; } = string.Empty;

        [NotNull]
        public string Audience { get; init; } = string.Empty;

        public int AccessExpiresInMinutes { get; init; }

        public int RefreshExpiresInMinutes { get; init; }
    }
}
