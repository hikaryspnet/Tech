using Microsoft.EntityFrameworkCore;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AuthDbContext _authDbContext;
        public SubscriptionRepository(AuthDbContext authDbContext)
        {
                _authDbContext = authDbContext;
        }
        public Task AddAsync(Subscription subscription)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _authDbContext.Subscriptions.AnyAsync(subscription => subscription.Id == id);
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

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
