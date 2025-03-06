using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EFCore.NamingConventions;

namespace SBL.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SblDbContext>
{
    public SblDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SblDbContext>();
        optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=main_db;Username=postgres;Password=mypassword")
            .UseSnakeCaseNamingConvention();

        return new SblDbContext(optionsBuilder.Options);
    }
}
