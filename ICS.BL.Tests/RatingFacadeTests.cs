using ICS.DAL;
using ICS.BL;
using Xunit;
using System.Threading.Tasks;
using System;
using ICS.BL.Facade.Interface;
using ICS.BL.Facade;
using ICS.BL.Models;
using System.Collections.ObjectModel;
using Xunit.Abstractions;

namespace ICS.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }


    [Fact]
    public async Task Create_WithoutActivity_Throws()
    {
        var model = new RatingDetailModel()
        {
            Id = Guid.Empty,
            points = 20,
            note = "note"
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _ratingFacadeSUT.SaveAsync(model));
    }
}