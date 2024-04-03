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
using ICS.DAL.Entities;

namespace ICS.BL.Tests;

public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }


    [Fact]
    public async Task Create_WithActivityStudent_DoesNotThrow()
    {
        var model = new RatingDetailModel()
        {
            Id = Guid.Empty,
            points = 20,
            note = "note",
            activityId = Guid.Empty,
            studentId = Guid.Empty,
            activity = new ActivityEntity()
            {
                Id = Guid.Empty,
                name = "name",
                start = DateTime.MinValue,
                end = DateTime.MinValue,
                room = "room"
            },
            student = new StudentEntity()
            {
                Id = Guid.Empty,
                firstName = "John",
                lastName = "Doe"
            }
        };
    }

        [Fact]
    public async Task GetAll_Single_SeededRating1()
    {
        var ratings = await _ratingFacadeSUT.GetAsync();
        var rating = ratings.Single(i => i.Id == RatingSeeds.Rating1.Id);
        DeepAssert.Equal(RatingModelMapper.MapToListModel(RatingSeeds.Rating1), rating);
    }

    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);

        //Act
        var returnedModel = await _ratingFacadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }
}