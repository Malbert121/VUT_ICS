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
            Points = 20,
            Note = "Note",
            StudentId = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            ActivityId = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485")
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
        var rating = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);

        //Act
        await _ratingFacadeSUT.SaveAsync(rating);

        //Assert
        var returnedModel = await _ratingFacadeSUT.GetAsync(rating.Id);
        DeepAssert.Equal(rating, returnedModel);
    }

    
    [Fact]
    public async Task SeededRating2_Update_RatingUpdated()
    {
        //Arrange
        var rating = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);
        rating.Points = 20;
        rating.Note += "Your evaluation was updated.";

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
        model.Points = 10;
        //Assert
        await _ratingFacadeSUT.SaveAsync(model);
    }
    
    [Fact]
    public async Task Update_Point_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = RatingModelMapper.MapToDetailModel(RatingSeeds.Rating1);
        detailModel.Points = 100;
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
    public async Task SearchBySubstringName_DoesNotThrow()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSearchAsync("Hermione", ActivitySeeds.ActivityWithTwoRatings.Id);

        Assert.Equal(2, ratingList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task SearchBySubstringNote_NonExistent_DoesNotThrow()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSearchAsync("Harry", ActivitySeeds.ActivityWithTwoRatings.Id);

        Assert.Empty(ratingList.ToObservableCollection());
    }

    [Fact]
    public async Task SortByDescendingId_Correct()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSortedAsync("byDescendingId", ActivitySeeds.ActivityWithTwoRatings.Id);
        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), ratingList.ToObservableCollection()[0].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), ratingList.ToObservableCollection()[1].Id);
    }

    [Fact]
    public async Task SortById_Correct()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSortedAsync("byId", ActivitySeeds.ActivityWithTwoRatings.Id);

        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), ratingList.ToObservableCollection()[1].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), ratingList.ToObservableCollection()[0].Id);
    }

    [Fact]
    public async Task SortByDescendingPoints_Correct()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSortedAsync("byDescendingPoints", ActivitySeeds.ActivityWithTwoRatings.Id);
        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), ratingList.ToObservableCollection()[0].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), ratingList.ToObservableCollection()[1].Id);
    }

    [Fact]
    public async Task SortByPoints_Correct()
    {
        var ratingList = await _ratingAppliedFacadeSUT.GetSortedAsync("byPoints", ActivitySeeds.ActivityWithTwoRatings.Id);
        Assert.Equal(Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"), ratingList.ToObservableCollection()[1].Id);
        Assert.Equal(Guid.Parse("a2e6849d-a158-4436-980c-7fc26b60c674"), ratingList.ToObservableCollection()[0].Id);
    }
}