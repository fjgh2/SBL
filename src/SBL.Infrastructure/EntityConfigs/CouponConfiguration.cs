using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Coupon)
            .HasForeignKey(o => o.CouponId);

        builder.Property(c => c.Id);
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(c => c.Percentage)
            .IsRequired();
        builder.Property(c => c.StartDate)
            .IsRequired()
            .HasColumnType("DATE");
        builder.Property(c => c.EndDate)
            .IsRequired()
            .HasColumnType("DATE");
        builder.Property(c => c.UsedCount)
            .IsRequired();
    }
}
