using ICS.BL.Tests;
using ICS.DAL;
using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Common.Tests.Factories;

public class DbContextTestingFactory(bool seedTestingData = false)
    : IDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext()
    {
        DbContextOptionsBuilder<SchoolContext> builder = new();
        builder.EnableSensitiveDataLogging();
        return new SchoolTestingContext(builder.Options, seedTestingData);
    }
}
