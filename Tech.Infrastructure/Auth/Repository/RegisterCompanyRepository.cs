using Microsoft.EntityFrameworkCore;
using Tech.Application.Auth.Interfaces;
using Tech.Core.Auth.Entities;

using Tech.Infrastructure.Auth.Data;

namespace Tech.Infrastructure.Auth.Repository
{
    public class RegisterCompanyRepository : IRegisterCompanyRepository
    {
        private readonly AuthDbContext _authDbContext;
        public RegisterCompanyRepository(AuthDbContext authDbContext) { _authDbContext = authDbContext; }
        public async Task InitializeCompanyAndUserRelationsAsync(Company company, User admin, int subscriptionId)
        {
            var moduleIdsTask = _authDbContext.SubscriptionModules.Where(sm => sm.SubscriptionId == subscriptionId).Select(sm => sm.ModuleId).ToListAsync();

            var roleIdTask = _authDbContext.Roles.Where(r => r.RoleType == Core.Auth.Enums.RoleType.CompanyAdmin).Select(r => r.Id).FirstAsync();

            await Task.WhenAll(moduleIdsTask, roleIdTask);

            var moduleIds = moduleIdsTask.Result;
            var roleId = roleIdTask.Result;

            await _authDbContext.UserRoles
                .AddAsync( new UserRole() 
                {
                    RoleId = roleId, 
                    UserId = admin.Id 
                });

            if(moduleIds.Count >  0)
            {
                foreach (var item in moduleIds)
                {
                    admin.UsersModules.Add(new UserModule() { ModuleId = item, UserId = admin.Id });
                    company.CompaniesModules.Add(new CompanyModule() { ModuleId = item, CompanyId = company.Id });
                }
            }

            _authDbContext.SaveChanges();
        }

        /// <summary>
        /// Checks whether a company with the specified name or email exists in the database
        /// </summary>
        /// <param name="companyName">The name of the company to check.</param>
        /// <param name="email">The email of the company to check.</param>
        /// <returns> Returns empty string if the company does not exist, or an error message if a company with the specified name or email already exists.</returns>
        public async Task<string?> ExistCompanyAsync(string companyName, string email)
        {
            string? message = string.Empty;

            var company = await _authDbContext.Companies
                .Where(c => c.Name == companyName || c.Email == email)
                .Select(c => new { c.Name, c.Email })
                .FirstOrDefaultAsync();

               if (company != null)
               {
                   if (company.Name == companyName)
                       return $"Company with name {companyName} already exists.";
                   if (company.Email == email)
                       return $"Company with email {email} already exists.";
               }

            return message;
        }
    }
}
