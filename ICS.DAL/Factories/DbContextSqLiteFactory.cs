using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<SchoolContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<SchoolContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public SchoolContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}