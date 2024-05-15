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
using ICS.Common.Tests.Seeds;
using ICS.DAL.Context;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ICS.BL.Tests.TestingOptions;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class RatingFacadeTests : FacadeTestsBase
{
    private readonly IRatingFacade _ratingFacadeSUT;

    private readonly RatingFacade _ratingAppliedFacadeSUT;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);

        _ratingAppliedFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }
    [Fact]
    public async Task Create_WithActivityStudent_DoesNotThrow()
    {
        //Arrange   
        var model = new RatingDetailModel()
        {
            Id = Guid.Parse("5ef12a97-24de-4df2-8c33-bef54679f333"),
            points = 20,
            note = "note",
            studentId = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            activityId = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485")
        };
        //Act & Assert
        var _ = await _ratingFacadeSUT.SaveAsync(model);
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

    [Fact]
    public async Task GetById_SeededRating2()
    {
        //Act
        var rating = await _ratingFacadeSUT.GetAsync(RatingSeeds.Rating2.Id);
        //Assert
        DeepAssert.Equal(RatingModelMapper.MapToDetailModel(RatingSeeds.Rating2), rating);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        //Act
        var rating = await _ratingFacadeSUT.GetAsync(RatingSeeds.EmptyEntity.Id);
        //Assert
        Assert.Null(rating);
    }


    [Fact]
    public async Task SeededRating1_DeleteById_Deleted()
    {
        //Act
        await _ratingFacadeSUT.DeleteAsync(RatingSeeds.Rating1.Id);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var actualRating = await dbxAssert.Rating.AnyAsync(i => i.Id == RatingSeeds.Rating1.Id);
        Assert.False(actualRating);
    }
    
    [Fact]
    public async Task NewRating_Insert_RatingAdded()
    {
        //Arrange
        var rating = new RatingDetailModel()
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aae4f7d"),
            points = 20,
            note = "note",
            Student = "Harry Potter",
            studentId = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            activityId = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
        };

        //Act
        rating = await _ratingFacadeSUT.SaveAsync(rating);

        //Assert
        var returnedModel = await _ratingFacadeSUT.GetAsync(rating.Id);
        DeepAssert.Equal(rating, returnedModel);
    }

    
    [Fact]
    public async Task SeededRating2_Update_RatingUpdated()
    {
        //Arrange
        var rating = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);
        rating.points = 20;
        rating.note += "Your evaluation was updated.";

        //Act
        await _ratingFacadeSUT.SaveAsync(rating);

        //Assert
        var returnedModel = await _ratingFacadeSUT.GetAsync(rating.Id);
        DeepAssert.Equal(rating, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var model = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);
        //Act
        model.points = 10;
        //Assert
        await _ratingFacadeSUT.SaveAsync(model);
    }
    
    [Fact]
    public async Task Update_Point_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);
        detailModel.points = 100;
        //Act
        await _ratingFacadeSUT.SaveAsync(detailModel);
        //Assert
        var returnedModel = await _ratingFacadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _ratingFacadeSUT.DeleteAsync(RatingSeeds.Rating2.Id);
    }

    [Fact]
    public async Task DeleteById_FromEmptySeeded_Throws()
    {
        //Arrange & Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _ratingFacadeSUT.DeleteAsync(RatingSeeds.EmptyEntity.Id));
    }

    [Fact]
    public async Task Delete_NonExisting_Throws()
    {
        //Arrange
        var model = RatingModelMapper.MapToDetailModel(RatingSeeds.RatingDelete);
        model.Id = Guid.NewGuid();

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _ratingFacadeSUT.DeleteAsync(model.Id));
    }

    [Fact]
    public async Task SearchBySubstringNote_DoesNotThrow()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSearchAsync("Good");

        Assert.Equal(3, ratingList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task SearchBySubstringNote_NonExistent_DoesNotThrow()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSearchAsync("Bad");

        Assert.Empty(ratingList.ToObservableCollection());
    }

    [Fact]
    public async Task SortByDescendingId_Correct()
    {
        var activityList = await _ratingAppliedFacadeSUT.GetSortedAsync("byDescendingId");

        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), activityList.ToObservableCollection()[0].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), activityList.ToObservableCollection()[1].Id);
        Assert.Equal(Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"), activityList.ToObservableCollection()[2].Id);
        Assert.Equal(Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"), activityList.ToObservableCollection()[3].Id);
    }

    [Fact]
    public async Task SortById_Correct()
    {
        var activityList = await _ratingAppliedFacadeSUT.GetSortedAsync("byId");

        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), activityList.ToObservableCollection()[3].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), activityList.ToObservableCollection()[2].Id);
        Assert.Equal(Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"), activityList.ToObservableCollection()[1].Id);
        Assert.Equal(Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"), activityList.ToObservableCollection()[0].Id);
    }

    [Fact]
    public async Task SortByDescendingPoints_Correct()
    {
        var activityList = await _ratingAppliedFacadeSUT.GetSortedAsync("byDescendingPoints");

        Assert.Equal(10, activityList.ToObservableCollection()[0].points);
        Assert.Equal(5, activityList.ToObservableCollection()[1].points);
        Assert.Equal(5, activityList.ToObservableCollection()[2].points);
        Assert.Equal(4, activityList.ToObservableCollection()[3].points);
    }

    [Fact]
    public async Task SortByPoints_Correct()
    {
        var activityList = await _ratingAppliedFacadeSUT.GetSortedAsync("byPoints");

        Assert.Equal(10, activityList.ToObservableCollection()[3].points);
        Assert.Equal(5, activityList.ToObservableCollection()[2].points);
        Assert.Equal(5, activityList.ToObservableCollection()[1].points);
        Assert.Equal(4, activityList.ToObservableCollection()[0].points);
    }
}