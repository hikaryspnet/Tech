using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();

            builder.Property(c => c.Email).HasMaxLength(256).IsRequired();
            builder.Property(c => c.PasswordHash).HasMaxLength(60).IsRequired();
            builder.Property(c => c.IsAdmin).HasColumnType("BIT").HasDefaultValue(false).IsRequired();

            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);

            builder
             .HasOne(u => u.Company)
             .WithMany(c => c.Employees)
             .HasForeignKey(u => u.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);

             builder
             .HasMany(u => u.UsersRoles)
             .WithOne(ur => ur.User)
             .HasForeignKey(ur => ur.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            builder
            .HasMany(u => u.UsersModules)
            .WithOne(um => um.User)
            .HasForeignKey(um => um.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder
             .HasMany(u => u.UsersRefreshTokens)
             .WithOne(urt => urt.User)
             .HasForeignKey(u => u.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

 
}
