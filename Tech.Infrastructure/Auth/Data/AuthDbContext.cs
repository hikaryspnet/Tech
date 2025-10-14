using Microsoft.EntityFrameworkCore;
using Tech.Core.Auth.Entities;
using Tech.Infrastructure.Auth.Data.EntityConfiguration;

namespace Tech.Infrastructure.Auth.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<CompanyModule> CompanyModules { get; set; }
        public DbSet<SubscriptionModule> SubscriptionModules { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("auth");

            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());

            modelBuilder.Entity<CompanyModule>().ToTable(nameof(CompanyModule)).HasKey(cm => new { cm.CompanyId, cm.ModuleId });

            modelBuilder.Entity<SubscriptionModule>().ToTable(nameof(SubscriptionModule)).HasKey(sm => new { sm.SubscriptionId, sm.ModuleId });

            modelBuilder.Entity<UserModule>().ToTable(nameof(UserModule)).HasKey(um => new { um.UserId, um.ModuleId });

            modelBuilder.Entity<UserRefreshToken>().ToTable(nameof(UserRefreshToken)).HasKey(urt => new { urt.UserId, urt.RefreshTokenId });

            modelBuilder.Entity<UserRole>().ToTable(nameof(UserRole)).HasKey(ur => new { ur.UserId, ur.RoleId });
                        
            modelBuilder.Entity<RolePermission>().ToTable(nameof(RolePermission)).HasKey(rp => new { rp.RoleId, rp.PermissionId });
        }
    }
}
