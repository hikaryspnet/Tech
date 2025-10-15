using Microsoft.EntityFrameworkCore;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _context;

        public RefreshTokenRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken, int userId)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();
            
            await _context.UserRefreshTokens.AddAsync(new UserRefreshToken() { RefreshTokenId = refreshToken.Id, UserId = userId});
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
