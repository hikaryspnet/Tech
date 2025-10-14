using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(256).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);
            builder.Property(m => m.RoleType).HasConversion<int>().IsRequired();


            builder.HasMany(r => r.UserRole)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Permissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
