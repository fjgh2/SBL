using Microsoft.EntityFrameworkCore;

namespace SBL.Infrastructure;

public class DesignTimeDbContextFactory
{
    public SblDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SblDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=3306;Database=laundry;User=myuser;Password=mypassword;");

        return new SblDbContext(optionsBuilder.Options);
    }
}
