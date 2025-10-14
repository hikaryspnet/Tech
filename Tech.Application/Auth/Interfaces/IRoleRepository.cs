using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name);
        Task SaveChanges();
    }
}
