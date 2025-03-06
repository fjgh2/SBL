using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.HasKey(bi => bi.Id);
        
        // builder.HasOne(bi => bi.User)
        //     .WithMany(u => u.BasketItems)
        //     .HasForeignKey(bi => bi.UserId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(bi => bi.Product)
            .WithMany(s => s.BasketItems)
            .HasForeignKey(bi => bi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(bi => bi.Id);
        builder.Ignore(bi => bi.Total);
        builder.Property(bi => bi.Quantity)
            .IsRequired();
        builder.Property(bi => bi.ProductId);
        builder.Property(bi => bi.UserId);
    }
}
