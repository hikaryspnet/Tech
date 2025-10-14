using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(256).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);
            builder.Property(m => m.Type).HasConversion<int>().IsRequired();

            builder.HasMany(p => p.Roles).WithOne(rp => rp.Permission).HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
