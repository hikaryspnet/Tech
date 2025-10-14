using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface ICompanyRepository
    {
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Company company);
        Task SaveChanges();

    }
}
