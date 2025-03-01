using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBL.Domain.Entities;

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

        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(SblDbContext)));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql("Server=localhost;Port=3306;Database=laundry;User=myuser;Password=mypassword;");
    }
}