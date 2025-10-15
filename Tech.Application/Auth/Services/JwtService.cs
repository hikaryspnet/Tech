using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tech.Application.Auth.DTOs;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Entities;
using Tech.Core.Auth.Common.Result;

namespace Tech.Infrastructure.Auth.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public JwtService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new NullReferenceException("Jwt sercret is not found");
            _issuer = configuration["Jwt:Issuer"] ?? throw new NullReferenceException("Jwt issuer is not found");
            _audience = configuration["Jwt:Audience"] ?? throw new NullReferenceException("Jwt audience is not found");
            _refreshTokenRepository = refreshTokenRepository;
        }

        public (Result<RefreshToken> refreshTokenResultEntity, string accessToken, string refreshToken) GenerateToken(User user)
        {
            var claims = new[]
             {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("companyId", user.CompanyId.ToString()),
                new Claim("isAdmin", user.IsAdmin.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            var refreshToken = Guid.NewGuid().ToString();
            //var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            Result<RefreshToken> refreshTokenResult = RefreshToken.Create(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));
            
            return (refreshTokenResult, new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken);

            //return (new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken, refreshTokenEntity);

        }

        public async Task<int?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (token == null || !token.IsValid())
                return null;

            return token.UserId;
        }

    }
}