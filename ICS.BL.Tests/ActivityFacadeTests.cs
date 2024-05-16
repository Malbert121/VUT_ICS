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
            Name = "Name",
            Start = DateTime.MinValue,
            End = DateTime.MinValue,
            Room = "Room",
            ActivityTypeTag = "Tag",
            SubjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Description = "Description",
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
            Name = "Name",
            Start = DateTime.MinValue,
            End = DateTime.MinValue,
            Room = "Room",
            ActivityTypeTag = "Tag",
            SubjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Description = "Description",
            Ratings = new ObservableCollection<RatingListModel>()
            {
                new RatingListModel
                {
                    Id = Guid.Empty,
                    Points = 0,
                    StudentId = Guid.Empty,
                    ActivityId = Guid.Empty
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
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Ratings =
            [
                new RatingListModel
                {
                    Id = Guid.Empty,
                    Points = 0,
                    StudentId = Guid.Empty,
                    ActivityId = Guid.Empty
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
            Name = "Name",
            Start = DateTime.MinValue,
            End = DateTime.MinValue,
            Room = "Room",
            ActivityTypeTag = "Tag",
            SubjectId = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Description = "Description",
            Ratings = new ObservableCollection<RatingListModel>()
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
        detailModel.Name = "Changed activity Name";

        //Act & Assert
        await _activityFacadeSUT.SaveAsync(detailModel with { Ratings = default! });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.Name = "Changed activity Name 1";

        //Act
        await _activityFacadeSUT.SaveAsync(detailModel with { Ratings = default! });

        //Assert
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveRatings_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.Ratings.Clear();

        //Act
        await _activityFacadeSUT.SaveAsync(detailModel with { Ratings = default! });

        //Assert
        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity), returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneNonExistingRating_FromSeeded_Throws()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.ActivityWithTwoRatings);
        detailModel.Ratings.Remove(detailModel.Ratings.First());

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _activityFacadeSUT.SaveAsync(detailModel));

    }


    [Fact]
    public async Task Update_RemoveOneOfRating_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.PotionsActivity);
        detailModel.Ratings.Remove(detailModel.Ratings.First());

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
        var returnedList = SubjectModelMapper.MapToDetailModel(SubjectSeeds.potions).Activity;

        //Assert
        Assert.Equal(activityList.ToObservableCollection().Count, returnedList.Count);
    }

    [Fact]
    public async Task SearchBySubstringName_NotThrow()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("Potions", SubjectSeeds.potions.Id);

        Assert.Equal(4, activityList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task SearchBySubstringNameOneResult_NotThrow()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("defence", SubjectSeeds.potions.Id);

        Assert.Single(activityList.ToObservableCollection());
    }
    [Fact]
    public async Task SearchNonExistingActivity_ReturnEmptyCollection()
    {
        var newGuid = Guid.NewGuid();
        var activityList = await _activityAppliedFacadeSUT.GetSearchAsync("Super Puper Lecture", newGuid);

        Assert.Equal(activityList.ToObservableCollection(), []);
    }

    [Fact]
    public async Task FilterByTime_OnlyStart()
    {
        var activityList = await _activityAppliedFacadeSUT.GetFilteredAsync(SubjectSeeds.potions.Id, new DateTime(2021, 10, 11, 10, 0, 0));

        Assert.Equal(2, activityList.ToObservableCollection().Count);
    }
    [Fact]
    public async Task FilterByTime_StartAndEnd()
    {
        var activityList = await _activityAppliedFacadeSUT.GetFilteredAsync(SubjectSeeds.potions.Id, new DateTime(2021, 10, 7, 10, 0, 0), new DateTime(2021, 10, 12, 10, 0, 0));

        Assert.Equal(3, activityList.ToObservableCollection().Count);
    }
    [Fact]
    public async Task SortByDescendingId()
    {
        var activityList = await _activityAppliedFacadeSUT.GetSortedAsync("byDescendingId", SubjectSeeds.potions.Id);

        Assert.Equal(6, activityList.ToObservableCollection().Count);
    }
    private static void FixIds(ActivityDetailModel expectedModel, ActivityDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var ratingModel in returnedModel.Ratings)
        {
            var ratingDetailModel = expectedModel.Ratings.FirstOrDefault(i =>
                Math.Abs(i.Points - ratingModel.Points) <= 0);

            if (ratingDetailModel != null)
            {
                ratingModel.Id = ratingDetailModel.Id;
                ratingModel.StudentId = ratingDetailModel.StudentId;
                ratingModel.ActivityId = ratingModel.ActivityId;
            }
        }
    }
}