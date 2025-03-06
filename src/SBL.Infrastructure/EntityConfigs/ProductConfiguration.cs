using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.HasMany(p => p.BasketItems)
            .WithOne(bi => bi.Product)
            .HasForeignKey(bi => bi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(p => p.Feedbacks)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(p => p.Price)
            .IsRequired();
    }
}
