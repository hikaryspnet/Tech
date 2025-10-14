using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface IJwtService
    {
        Task<(string accessToken, string refreshToken, RefreshToken refreshTokenEntity)> GenerateTokensAsync(User user);
        Task<int?> ValidateRefreshTokenAsync(string refreshToken);
    }
}
