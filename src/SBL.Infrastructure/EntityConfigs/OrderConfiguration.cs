using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Primary Key
        builder.HasKey(o => o.Id);

        // Relationships
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Coupon)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CouponId)
            .OnDelete(DeleteBehavior.SetNull);

        // Props
        builder.Property(o => o.Id);
        builder.Property(o => o.Status)
            .IsRequired();
        builder.Property(o => o.Subtotal)
            .IsRequired();
        builder.Property(o => o.PaymentStatus)
            .IsRequired();
        builder.Property(o => o.HasCoupon)
            .IsRequired();
        builder.Property(o => o.Discount)
            .IsRequired();
        builder.Property(o => o.DeliveryFee)
            .IsRequired();
        builder.Property(o => o.CouponId);
        builder.Property(o => o.DeliveredDate)
            .HasColumnType("DATE");
        builder.Property(o => o.UserId)
            .IsRequired(false);
        builder.Property(o => o.Address);
        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("TIMESTAMP");
    }
}
