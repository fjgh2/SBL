using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(v => v.Id);

        // builder.HasOne(v => v.User)
        //     .WithMany(u => u.Votes)
        //     .HasForeignKey(v => v.UserId)
        //     .IsRequired() 
        //     .OnDelete(DeleteBehavior.Cascade);
        builder.Property(v => v.Id);
        builder.Property(v => v.Count)
            .IsRequired();
        builder.Property(v => v.UserId)
            .IsRequired();
        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("TIMESTAMP");
        builder.Property(o => o.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("TIMESTAMP");
    }
}
