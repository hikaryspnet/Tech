using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIdAsync(int id);
        Task AddAsync(Subscription subscription);
        Task SaveChanges();
    }
}
