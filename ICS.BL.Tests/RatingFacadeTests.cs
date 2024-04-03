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
using ICS.Common.Tests2.Seeds;

namespace ICS.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }


    [Fact]
    public async Task Create_WithoutActivity_DoesNotThrow()
    {
        var model = new RatingDetailModel()
        {
            Id = Guid.Empty,
            points = 20,
            note = "note"
        };

        var _ = await _ratingFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task GetAll_Single_SeededRating1()
    {
        var ratings = await _ratingFacadeSUT.GetAsync();
        var rating = ratings.Single(i => i.Id == RatingSeeds.Rating1.Id);
        DeepAssert.Equal(RatingModelMapper.MapToListModel(RatingSeeds.Rating1), rating);
    }
}