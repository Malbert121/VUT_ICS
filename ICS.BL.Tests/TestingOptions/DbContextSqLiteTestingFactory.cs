using ICS.DAL;
using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Tests.TestingOptions;

public class DbContextSqLiteTestingFactory(string? databaseName, bool seedTestingData = false)
    : IDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext()
    {

        DbContextOptionsBuilder<SchoolContext> builder = new();

        builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        return new SchoolTestingContext(builder.Options, seedTestingData);

    }
}