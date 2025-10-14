using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    public class ModuleEntityConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(256).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);

            builder.Property(m => m.ModuleType).HasConversion<int>().IsRequired();

            builder.HasMany(m => m.CompaniesModules).WithOne(cm => cm.Module).HasForeignKey(cm => cm.ModuleId).OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(m => m.SubscriptionsModules).WithOne(sm => sm.Module).HasForeignKey(sm => sm.ModuleId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UsersModules).WithOne(um => um.Module).HasForeignKey(um => um.ModuleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
