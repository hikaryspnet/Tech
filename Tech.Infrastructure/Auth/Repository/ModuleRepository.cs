using Microsoft.EntityFrameworkCore;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AuthDbContext _authDbContext;
        public ModuleRepository(AuthDbContext authDbContext)
        {
                _authDbContext = authDbContext;
        }
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _authDbContext.Modules.AnyAsync(m => m.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _authDbContext.Modules.AnyAsync(m => m.Id == id);
        }

        public async Task AddAsync()
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
