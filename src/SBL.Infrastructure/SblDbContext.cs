using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBL.Domain.Entities;
using SBL.Infrastructure.EntityConfigs;

namespace SBL.Infrastructure;

public class SblDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public SblDbContext(DbContextOptions<SblDbContext> options) : base(options)
    {
    }

    public DbSet<BasketItem> BasketItems { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());

        builder.Entity<User>().ToTable("users");
        builder.Entity<IdentityRole<int>>().ToTable("roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
        builder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");
        // builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SblDbContext))!);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=main_db;Username=postgres;Password=mypassword")
                .UseSnakeCaseNamingConvention();
        }
    }
}