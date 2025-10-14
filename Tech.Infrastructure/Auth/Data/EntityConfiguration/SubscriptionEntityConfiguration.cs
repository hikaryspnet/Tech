using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tech.Core.Auth.Entities;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    internal class SubscriptionEntityConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(256).IsRequired(false);
            builder.Property(c => c.CreatedAt).HasColumnType("DATETIME2").IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("DATETIME2").IsRequired(false);
            builder.Property(c => c.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(m => m.SubscriptionType).HasConversion<int>().IsRequired();


            builder.HasMany(s => s.Companies)
                .WithOne(c => c.Subscription)
                .HasForeignKey(c => c.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(s => s.IncludesModules)
                .WithOne(im => im.Subscription)
                .HasForeignKey(im => im.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
