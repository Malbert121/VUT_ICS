using ICS.Common.Tests.Seeds;
using ICS.DAL;
using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Tests;

public class SchoolTestingContext(DbContextOptions contextOptions, bool seedTestingData = false)
    : SchoolContext(contextOptions, seedDemoData: false)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (seedTestingData)
        {
            ActivitySeeds.Seed(modelBuilder);
            RatingSeeds.Seed(modelBuilder);
            StudentSeeds.Seed(modelBuilder);
            SubjectSeeds.Seed(modelBuilder);
        }
    }
}