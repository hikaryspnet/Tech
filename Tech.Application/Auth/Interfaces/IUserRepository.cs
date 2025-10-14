using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task SaveChanges();
    }
}
