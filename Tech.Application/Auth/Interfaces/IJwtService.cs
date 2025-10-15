using Tech.Core.Auth.Entities;
using Tech.Core.Auth.Common.Result;

namespace Tech.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        //Task<(string accessToken, string refreshToken, RefreshToken refreshTokenEntity)> GenerateToken(User user);
        Task<int?> ValidateRefreshTokenAsync(string refreshToken);
        (Result<RefreshToken> refreshTokenResultEntity, string accessToken, string refreshToken) GenerateToken(User user);
    }
}
