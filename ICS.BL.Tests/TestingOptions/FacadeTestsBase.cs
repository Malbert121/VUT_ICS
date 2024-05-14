using ICS.DAL;
using Xunit;
using System.Threading.Tasks;
using System;
using Xunit.Abstractions;
using ICS.BL.Mappers;
using Microsoft.EntityFrameworkCore;
using ICS.DAL.UnitOfWork;
using ICS.DAL.Context;

namespace ICS.BL.Tests.TestingOptions;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        RatingModelMapper = new RatingModelMapper();
        ActivityModelMapper = new ActivityModelMapper(RatingModelMapper);

        StudentSubjectModelMapper = new StudentSubjectModelMapper();

        SubjectModelMapper = new SubjectModelMapper(ActivityModelMapper, StudentSubjectModelMapper);
        StudentModelMapper = new StudentModelMapper(StudentSubjectModelMapper);



        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<SchoolContext> DbContextFactory { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    public StudentSubjectModelMapper StudentSubjectModelMapper { get; }
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