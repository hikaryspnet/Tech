using Microsoft.EntityFrameworkCore;
using Tech.Core.Auth.Entities;
using Tech.Core.Auth.Enums;

namespace Tech.Infrastructure.Auth.Data
{
    public static class AuthDbInitializer
    {
        public static async Task SeedAsync(AuthDbContext context)
        {
            if (await context.Subscriptions.AnyAsync())
            {
                return;
            }

            if (!context.Modules.Any())
            {
                var modules = new List<Module>
                {
                    Module.Create("Profile", "User personal and company profiles", ModuleType.Profile).Value!,
                    Module.Create("CRM", "Customer relationship management", ModuleType.CRM).Value!,
                    Module.Create("ERP", "Enterprise resource planning", ModuleType.ERP).Value!
                };

                await context.Modules.AddRangeAsync(modules);
                await context.SaveChangesAsync();
            }

            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    Permission.Create("None", PermissionType.None, "No access").Value!,
                    Permission.Create("Read", PermissionType.Read, "Read access").Value!,
                    Permission.Create("Write", PermissionType.Write, "Create entities").Value!,
                    Permission.Create("Update", PermissionType.Update, "Modify entities").Value!,
                    Permission.Create("Delete", PermissionType.Delete, "Remove entities").Value!
                };

                await context.Permissions.AddRangeAsync(permissions);
                await context.SaveChangesAsync();
            }

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    Role.Create("Admin", RoleType.Admin, "Global system administrator").Value!,
                    Role.Create("Company Admin", RoleType.CompanyAdmin, "Administrator within company").Value!,
                    Role.Create("Developer", RoleType.Developer, "Developer of company projects").Value!,
                    Role.Create("Project Manager", RoleType.PM, "Manages projects and teams").Value!,
                    Role.Create("HR", RoleType.HR, "Handles recruitment and employee data").Value!
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            if (!context.Subscriptions.Any())
            {
                var subscriptions = new List<Subscription>
                {
                    Subscription.Create("Base", "Basic plan with Profile only", 10.00m, SubscriptionType.Base).Value!,
                    Subscription.Create("Public", "Includes CRM + Profile modules", 44.44m, SubscriptionType.Public).Value!,
                    Subscription.Create("Premium", "All modules available", 77.99m, SubscriptionType.Premium).Value!
                };

                await context.Subscriptions.AddRangeAsync(subscriptions);
                await context.SaveChangesAsync();
            }

            var allPermissions = context.Permissions.ToList();
            var allModules = context.Modules.ToList();
            var allRoles = context.Roles.ToList();
            var allSubscriptions = context.Subscriptions.ToList();

            foreach (var role in allRoles)
            {
                if (!role.Permissions.Any())
                {
                    var rolePermissions = new List<RolePermission>();

                    switch (role.RoleType)
                    {
                        case RoleType.Admin:
                        case RoleType.CompanyAdmin:
                            rolePermissions = allPermissions
                                .Select(p => new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = p.Id
                                }).ToList();
                            break;

                        case RoleType.Developer:
                            rolePermissions = allPermissions
                                .Where(p => p.Type != PermissionType.Delete)
                                .Select(p => new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = p.Id
                                }).ToList();
                            break;

                        case RoleType.PM:
                        case RoleType.HR:
                            rolePermissions = allPermissions
                                .Where(p => p.Type != PermissionType.Delete)
                                .Select(p => new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = p.Id
                                }).ToList();
                            break;
                    }

                    await context.RolePermissions.AddRangeAsync(rolePermissions);
                }
            }

            foreach (var subscription in allSubscriptions)
            {
                if (!context.SubscriptionModules.Any(sm => sm.SubscriptionId == subscription.Id))
                {
                    var modulesToAdd = subscription.SubscriptionType switch
                    {
                        SubscriptionType.Base => allModules.Take(1),      
                        SubscriptionType.Public => allModules.Take(2),    
                        SubscriptionType.Premium => allModules,           
                        _ => Enumerable.Empty<Module>()
                    };

                    var subscriptionModules = modulesToAdd.Select(m => new SubscriptionModule
                    {
                        SubscriptionId = subscription.Id,
                        ModuleId = m.Id
                    });

                    await context.SubscriptionModules.AddRangeAsync(subscriptionModules);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
