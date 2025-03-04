using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(u => u.BasketItems)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.Id);
        builder.Property(u => u.Role)
            .IsRequired();
        
        // Identity-specific properties
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(150);
        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(150);
        builder.Property(u => u.EmailConfirmed)
            .IsRequired();
        builder.Property(u => u.SecurityStamp)
            .HasMaxLength(36);
        builder.Property(u => u.ConcurrencyStamp)
            .HasMaxLength(36);
        builder.Property(u => u.PhoneNumberConfirmed)
            .IsRequired();
        builder.Property(u => u.TwoFactorEnabled)
            .IsRequired();
        builder.Property(u => u.LockoutEnd);
        builder.Property(u => u.LockoutEnabled)
            .IsRequired();
        builder.Property(u => u.AccessFailedCount)
            .IsRequired();
    }
}
