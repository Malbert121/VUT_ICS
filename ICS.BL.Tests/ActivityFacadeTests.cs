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
using ICS.BL.Tests.TestingOptions;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    private readonly ActivityFacade _activityAppliedFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);

        _activityAppliedFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
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
            subjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
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
            subjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
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
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f486"),
            name = "Potions lecture",
            start = new DateTime(2021, 10, 10, 10, 0, 0),
            end = new DateTime(2021, 10, 10, 12, 0, 0),
            room = "A03",
            activityTypeTag = "POT",
            description = "Brewing a potion",
            subjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
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
            subjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
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
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.DeleteAsync(ActivitySeeds.EmptyActivity.Id));
    }

    [Fact]
    public async Task GetList_NotThrow()
    {
        //Arrange & Act
        var activityList = await _activityFacadeSUT.GetAsync();
        var returnedList = SubjectModelMapper.MapToDetailModel(SubjectSeeds.potions).activity;

        //Assert
        Assert.Equal(activityList.ToObservableCollection().Count, returnedList.Count);
    }

    [Fact]
    public async Task SearchBySubstringName_NotThrow()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("Potions");

        Assert.Equal(4, activityList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task SearchBySubstringNameOneResult_NotThrow()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("Dark");

        Assert.Single(activityList.ToObservableCollection());
    }
    [Fact]
    public async Task SearchNonExistingActivity_ReturnEmptyCollection()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("Super Puper Lecture");

        Assert.Equal(activityList.ToObservableCollection(), []);
    }

    [Fact]
    public async Task FilterByTime_OnlyStart()
    {
        var activityList = await _activityAppliedFacadeSUT.GetFilteredAsync(new DateTime(2021, 10, 11, 10, 0, 0));

        Assert.Equal(2, activityList.ToObservableCollection().Count);
    }
    [Fact]
    public async Task FilterByTime_StartAndEnd()
    {
        var activityList = await _activityAppliedFacadeSUT.GetFilteredAsync(new DateTime(2021, 10, 7, 10, 0, 0), new DateTime(2021, 10, 12, 10, 0, 0));

        Assert.Equal(3, activityList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task SortByDescendingId()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSortedAsync("byDescendingId");
        //TODO give normal assert

        Assert.Equal(6, activityList.ToObservableCollection().Count);
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