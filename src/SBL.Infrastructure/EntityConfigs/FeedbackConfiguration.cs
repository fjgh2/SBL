using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id);
        builder.Property(f => f.Rating)
            .IsRequired();
        builder.Property(f => f.Comment)
            .HasMaxLength(1000);
        builder.Property(f => f.ProductId)
            .IsRequired();
        builder.Property(f => f.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("TIMESTAMP");
        builder.ToTable(t => t
            .HasCheckConstraint("CK_Feedback_Rating", "Rating >= 1 AND Rating <= 5"));
    }
}
