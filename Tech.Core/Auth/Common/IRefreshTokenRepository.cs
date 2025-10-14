using Tech.Core.Auth.Entities;

namespace Tech.Core.Auth.Common
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task SaveChanges();

    }
}
