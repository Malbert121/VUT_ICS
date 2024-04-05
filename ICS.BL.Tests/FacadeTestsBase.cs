using ICS.DAL;
using ICS.BL;
using Xunit;
using System.Threading.Tasks;
using System;
using Xunit.Abstractions;
using ICS.BL.Mappers;
using Microsoft.EntityFrameworkCore;
using ICS.DAL.UnitOfWork;
using ICS.DAL.Context;

namespace ICS.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName, seedTestingData: true);

        RatingModelMapper = new RatingModelMapper();
        ActivityModelMapper = new ActivityModelMapper(RatingModelMapper);
        StudentModelMapper = new StudentModelMapper(SubjectModelMapper);
        SubjectModelMapper = new SubjectModelMapper(ActivityModelMapper, StudentModelMapper);

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<SchoolContext> DbContextFactory { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected RatingModelMapper RatingModelMapper { get; }
    protected StudentModelMapper StudentModelMapper { get; }
    protected SubjectModelMapper SubjectModelMapper { get; }

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}