using Microsoft.EntityFrameworkCore;
using Tech.Core.Auth.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tech.Infrastructure.Auth.Data.EntityConfiguration
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Fingerprint)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(d => d.Fingerprint)
                .IsUnique();

            builder.Property(d => d.DeviceType)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(d => d.OperatingSystem)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(d => d.Browser)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(d => d.IpAddress)
                   .IsRequired()
                   .HasMaxLength(45);

            builder
                .HasOne(d => d.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
