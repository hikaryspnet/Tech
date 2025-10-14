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
            var moduleIds = await _authDbContext.SubscriptionModules.Where(sm => sm.SubscriptionId == subscriptionId).Select(sm => sm.ModuleId).ToListAsync();

            var roleId = await _authDbContext.Roles.Where(r => r.RoleType == Core.Auth.Enums.RoleType.CompanyAdmin).Select(r => r.Id).FirstAsync();

            await _authDbContext.UserRoles
                .AddAsync( new UserRole() 
                {
                    RoleId = roleId, 
                    UserId = admin.Id 
                });

            if (moduleIds.Any())
            {
                for (int i = 0; i < moduleIds.Count; i++)
                {
                    admin.UsersModules.Add(new UserModule() { ModuleId = moduleIds[i], UserId = admin.Id });
                    company.CompaniesModules.Add(new CompanyModule() { ModuleId = moduleIds[i], CompanyId = company.Id });
                }
            }

            _authDbContext.SaveChanges();
        }

        /// <summary>
        /// Checks whether a company with the specified name or email exists in the database
        /// </summary>
        /// <param name="companyName">The name of the company to check.</param>
        /// <param name="email">The email of the company to check.</param>
        /// <returns> Returns null if the company does not exist, or an error message if a company with the specified name or email already exists.</returns>
        public async Task<string?> ExistCompanyAsync(string companyName, string email)
        {
            string? message;

            message = await _authDbContext.Companies.AnyAsync(m => m.Name == companyName)
                ? $"Company with name {companyName} is already exist."
                : null;

            if (!string.IsNullOrEmpty(message)) return message;

            message = await _authDbContext.Companies.AnyAsync(m => m.Email == email)
                ? $"Company with email {email} is already exist."
                : null;
            
            return message;
        }

        //public async Task SaveChangesAsync()
        //{
        //    await _authDbContext.SaveChangesAsync();
        //}


        //public async Task RegisterCompany(int companyId, int adminId, int subscriptionId)
        //{
        //    var moduleIds = await _authDbContext.SubscriptionModules.Where(sm => sm.SubscriptionId == subscriptionId).Select(sm => sm.ModuleId).ToListAsync();

        //    var roleId = await _authDbContext.Roles.Where(r => r.RoleType == Core.Auth.Enums.RoleType.CompanyAdmin).Select(r => r.Id).FirstAsync();

        //    await _authDbContext.UserRoles
        //        .AddAsync(new UserRole()
        //        {
        //            RoleId = roleId,
        //            UserId = admin.Id
        //        });

        //    if (moduleIds.Any())
        //    {
        //        for (int i = 0; i < moduleIds.Count; i++)
        //        {
        //            admin.UsersModules.Add(new UserModule() { ModuleId = moduleIds[i], UserId = admin.Id });
        //            company.CompaniesModules.Add(new CompanyModule() { ModuleId = moduleIds[i], CompanyId = company.Id });
        //        }
        //    }

        //    await _authDbContext.SaveChangesAsync();
        //}
    }
}
