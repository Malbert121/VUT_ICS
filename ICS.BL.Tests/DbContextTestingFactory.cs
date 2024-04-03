using ICS.BL.Tests;
using ICS.DAL;
using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory(bool seedTestingData = false)
    : IDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext()
    {
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();

        return new SchoolTestingContext(options, seedTestingData);
    }
}
