using ICS.DAL;
using ICS.BL;
using Xunit;
using System.Threading.Tasks;
using System;
using ICS.BL.Facade.Interface;
using ICS.BL.Facade;
using ICS.BL.Models;
using System.Collections.ObjectModel;
using ICS.DAL.Context;
using Xunit.Abstractions;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using ICS.Common.Tests.Seeds;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }

    [Fact]
    public async Task Create_WithWithoutRating_EqualsCreated()
    {
        //Arrange
        var model = new ActivityDetailModel()
        {
            name = "name",
            start = DateTime.MinValue,
            end = DateTime.MinValue,
            room = "room",
            activityTypeTag = "Tag",
            subjectId = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f333"),
            subject = new SubjectListModel()
            {
                Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f333"),
                abbreviation = "IDS",
                name = "Database systems"
            },
            description = "description",
        };

        //Act
        var returnedModel = await _activityFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingRating_Throws()
    {
        var model = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            name = "name",
            start = DateTime.MinValue,
            end = DateTime.MinValue,
            room = "room",
            activityTypeTag = "Tag",
            subjectId = Guid.Empty,
            description = "description",
            ratings = new ObservableCollection<RatingListModel>()
            {
                new RatingListModel
                {
                    Id = Guid.Empty,
                    points = 0,
                    studentId = Guid.Empty,
                    activityId = Guid.Empty
                }
            }
        };
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithNonExistingAndExistigRating_Throws()
    {
        var model = new ActivityDetailModel()
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
            name = "Potions lecture",
            start = new DateTime(2021, 10, 10, 10, 0, 0),
            end = new DateTime(2021, 10, 10, 12, 0, 0),
            room = "A03",
            activityTypeTag = "POT",
            description = "Brewing a potion",
            subjectId = Guid.Empty,
            subject = SubjectModelMapper.MapToListModel(SubjectSeeds.potions),
            ratings =
            [
                new RatingListModel
                {
                    Id = Guid.Empty,
                    points = 0,
                    studentId = Guid.Empty,
                    activityId = Guid.Empty
                }
                ,

                RatingModelMapper.MapToListModel(RatingSeeds.Rating1)

            ]
        };
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithEmptyListRating_DoesNotThrow()
    {

        var model = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            name = "name",
            start = DateTime.MinValue,
            end = DateTime.MinValue,
            room = "room",
            activityTypeTag = "Tag",
            subjectId = Guid.Empty,
            subject = new SubjectListModel()
            {
                Id = Guid.Empty,
                abbreviation = "IDS",
                name = "Database systems"
            },
            description = "description",
            ratings = new ObservableCollection<RatingListModel>()
        };

        await _activityFacadeSUT.SaveAsync(model);

    }


    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);

        //Act
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task GetAll_FromSeeded_ContainsSeeded()
    {
        //Arrange
        var listModel = ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityWithTwoRatings);

        //Act
        var returnedModel = await _activityFacadeSUT.GetAsync();

        //Assert
        Assert.Contains(listModel, returnedModel);
    }


    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.name = "Changed activity name";

        //Act & Assert
        await _activityFacadeSUT.SaveAsync(detailModel with { ratings = default! });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.name = "Changed activity name 1";

        //Act
        await _activityFacadeSUT.SaveAsync(detailModel with { ratings = default! });

        //Assert
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveRatings_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.ratings.Clear();

        //Act
        await _activityFacadeSUT.SaveAsync(detailModel with { ratings = default! });

        //Assert
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity), returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneNonExistingRating_FromSeeded_Throws()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.ActivityWithTwoRatings);
        detailModel.ratings.Remove(detailModel.ratings.First());

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.SaveAsync(detailModel));

    }


    [Fact]
    public async Task Update_RemoveOneOfRating_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.ratings.Remove(detailModel.ratings.First());

        //Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.SaveAsync(detailModel));

        //Assert
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity), returnedModel);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _activityFacadeSUT.DeleteAsync(ActivitySeeds.PotionsActivity.Id);
    }

    [Fact]
    public async Task DeleteById_FromEmptySeeded_Throws()
    {

        //Arrange & Act & Assert
        await  Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.DeleteAsync(ActivitySeeds.EmptyActivity.Id));
    }


    private static void FixIds(ActivityDetailModel expectedModel, ActivityDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var ratingModel in returnedModel.ratings)
        {
            var ratingDetailModel = expectedModel.ratings.FirstOrDefault(i =>
                Math.Abs(i.points - ratingModel.points) <= 0);

            if (ratingDetailModel != null)
            {
                ratingModel.Id = ratingDetailModel.Id;
                ratingModel.studentId = ratingDetailModel.studentId;
                ratingModel.activityId = ratingModel.activityId;
            }
        }
    }
}