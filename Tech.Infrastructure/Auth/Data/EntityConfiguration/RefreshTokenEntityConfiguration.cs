using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.ExpiresAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.RevokedAt).HasColumnType("DATETIME2").IsRequired(false);

            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);

            builder.HasMany(rt => rt.UsersRefreshTokens)
                .WithOne(urt => urt.RefreshToken)
                .HasForeignKey(urt => urt.RefreshTokenId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
