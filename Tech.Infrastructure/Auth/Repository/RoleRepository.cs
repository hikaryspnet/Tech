using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthDbContext _context;

        public RoleRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Roles.AnyAsync(r => r.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id);
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

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
