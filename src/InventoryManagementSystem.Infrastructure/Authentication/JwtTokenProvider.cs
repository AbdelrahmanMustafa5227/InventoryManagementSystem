using InventoryManagementSystem.Application.Abstractions.Authentication;
using InventoryManagementSystem.Application.Helpers;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Authentication
{
    internal class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtOptions _options;
        private readonly AppDbContext _dbContext;

        public JwtTokenProvider(IOptions<JwtOptions> options, AppDbContext dbContext)
        {
            _options = options.Value;
            _dbContext = dbContext;
        }

        public JwtToken GenerateJwtToken(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = PersistRefreshToken(user.Id);
            return new JwtToken(accessToken, refreshToken);
        }

        public async Task<Result<JwtToken>> RefreshAccessToken(Guid refreshToken)
        {
            // Get refresh token from DB
            var TokenFromDb = await _dbContext.Set<RefreshToken>().FirstOrDefaultAsync(u => u.Token == refreshToken);
            if (TokenFromDb == null)
                return Result.Failure<JwtToken>(new Error("Invalid Refresh Token"));

            // Check if the refresh token is expired 
            if (TokenFromDb.ExpiresAt < DateTime.Now)
                return Result.Failure<JwtToken>(new Error("Expired Refresh Token"));

            // Generate new Access Tokens
            var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Id == TokenFromDb.UserId);
            if (user == null)
                return Result.Failure<JwtToken>(new Error("User not found"));

            var accessToken = GenerateAccessToken(user);

            return Result.Success(new JwtToken(accessToken, TokenFromDb.Token));
        }

        public async Task Revoke(long UserId)
        {
            await _dbContext.Set<RefreshToken>().Where(x => x.UserId == UserId).ExecuteDeleteAsync();
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email , user.Email),
                new Claim(ClaimTypes.Role , user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Audience = _options.Audience,
                Issuer = _options.Issuer,
                Expires = DateTime.Now.AddMinutes(_options.AccessExpiresInMinutes)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Guid PersistRefreshToken(long UserId)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid(),
                UserId = UserId,
                ExpiresAt = DateTime.Now.AddMinutes(_options.RefreshExpiresInMinutes)
            };

            _dbContext.Set<RefreshToken>().Add(refreshToken);
            _dbContext.SaveChanges();
            return refreshToken.Token;
        }
    }
}

