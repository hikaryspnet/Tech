using Microsoft.EntityFrameworkCore;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AuthDbContext _context;

        public CompanyRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Companies.AnyAsync(c => c.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Companies.AnyAsync(c => c.Id == id);
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
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
