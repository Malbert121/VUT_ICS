using ICS.BL.Tests;
using ICS.DAL;
using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Tests;

public class DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    : IDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext()
    {

        DbContextOptionsBuilder<SchoolContext> builder = new();

        builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        return new SchoolTestingContext(builder.Options, seedTestingData);

    }
}