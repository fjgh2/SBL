using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(o => o.Name);
        builder.Property(o => o.Description);
        builder.Property(o => o.Picture);
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
