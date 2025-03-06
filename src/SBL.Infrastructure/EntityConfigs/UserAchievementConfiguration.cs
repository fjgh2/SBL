using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBL.Domain.Entities;

namespace SBL.Infrastructure.EntityConfigs;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.HasKey(ua => new { ua.UserId, ua.AchievementId });

        builder.Property(ua => ua.AchievementId);
        builder.Property(ua => ua.UserId);
        builder.Property(ua => ua.CreatedAt);
    }
}
