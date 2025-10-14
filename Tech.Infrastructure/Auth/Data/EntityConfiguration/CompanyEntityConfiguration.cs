using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Email).IsUnique();
            
            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(256).IsRequired();
            builder.Property(c => c.CompanyAdminId).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);

            builder
                .HasOne(c => c.CompanyAdmin)
                .WithOne()
                .HasForeignKey<Company>(c => c.CompanyAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.Employees)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(c => c.CompaniesModules)
                .WithOne(cm => cm.Company)
                .HasForeignKey(cm => cm.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.Subscription)
                .WithMany(s => s.Companies)
                .HasForeignKey(c => c.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
