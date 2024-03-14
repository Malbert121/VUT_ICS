using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<CookBookDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<CookBookDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public CookBookDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}