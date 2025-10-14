using Tech.Core.Auth.Entities;

namespace Tech.Application.Auth.Interfaces
{
    public interface IRegisterCompanyRepository
    {
        public Task<string?> ExistCompanyAsync(string companyName, string email);
        public Task InitializeCompanyAndUserRelationsAsync(Company company, User admin, int subscriptionId);
    }
}
