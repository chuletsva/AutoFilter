using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Autofilter.Tests.Common.EF;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public const string ConnectionString = "Host=localhost;Port=5432;Database=autofilter;Username=postgres;Password=postgres;TrustServerCertificate=true";

    public AppDbContext CreateDbContext(string[]? args = default)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(ConnectionString);

        return new AppDbContext(options.Options);
    }
}