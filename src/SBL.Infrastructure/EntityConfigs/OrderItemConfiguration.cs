using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        
        builder.HasOne(oi => oi.Product)
            .WithMany(s => s.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(oi => oi.Id);
        builder.Property(oi => oi.Quantity)
            .IsRequired();
        builder.Property(oi => oi.Total)
            .IsRequired();
        builder.Property(oi => oi.ProductId);
        builder.Property(oi => oi.OrderId);
    }
}
