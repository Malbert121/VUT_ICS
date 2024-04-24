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

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
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
        //Arrange   
        var model = new RatingDetailModel()
        {
            Id = Guid.Empty,
            points = 20,
            note = "note",
            activityId = Guid.Parse("34a98f97-30de-4df2-8c33-bef54679f333"),
            studentId = Guid.Parse("5ef12a97-24de-4df2-8c33-bef54679f333"),
            activity = new ActivityListModel()
            {
                Id = Guid.Parse("34a98f97-30de-4df2-8c33-bef54679f333"),
                name = "name",
                start = DateTime.MinValue,
                end = DateTime.MinValue,
                room = "room",
                subjectId = SubjectSeeds.SubjectWithTwoStudents.Id
            },
            student = new StudentListModel()
            {
                Id = Guid.Parse("5ef12a97-24de-4df2-8c33-bef54679f333"),
                firstName = "John",
                lastName = "Doe"
            }
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

    //[Fact]
    //public async Task GetById_NonExistent()
    //{
    //    //Act
    //    var rating = await _ratingFacadeSUT.GetAsync(RatingSeeds.EmptyEntity.Id);
    //    //Assert
    //    Assert.Null(rating);
    //}


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
            Id = Guid.Empty,
            points = 20,
            note = "note",
            activityId = Guid.Parse("34a98f97-30de-4df2-8c33-bef54679f333"),
            studentId = Guid.Parse("5ef12a97-24de-4df2-8c33-bef54679f333"),
            activity = new ActivityListModel()
            {
                Id = Guid.Parse("34a98f97-30de-4df2-8c33-bef54679f333"),
                name = "name",
                start = DateTime.MinValue,
                end = DateTime.MinValue,
                room = "room",
                subjectId = SubjectSeeds.SubjectWithTwoStudents.Id
            },
            student = new StudentListModel()
            {
                Id = Guid.Parse("5ef12a97-24de-4df2-8c33-bef54679f333"),
                firstName = "John",
                lastName = "Doe"
            }
        };

        //Act
        rating = await _ratingFacadeSUT.SaveAsync(rating);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var ratingFromDb = await dbxAssert.Rating.SingleAsync(i => i.Id == rating.Id);
        DeepAssert.Equal(rating, RatingModelMapper.MapToDetailModel(ratingFromDb));
    }

    
    [Fact]
    public async Task SeededRating2_Update_IngredientUpdated()
    {
        //Arrange
        var rating = new RatingDetailModel()
        {
            Id = RatingSeeds.Rating1.Id,
            points = RatingSeeds.Rating1.Points,
            note = RatingSeeds.Rating1.Note,
            activityId = RatingSeeds.Rating1.ActivityId,
            studentId = RatingSeeds.Rating1.StudentId,
            activity = ActivityModelMapper.MapToListModel(RatingSeeds.Rating1.Activity),
            student = StudentModelMapper.MapToListModel(RatingSeeds.Rating1.Student)
        };
        rating.points = 20;
        rating.note += "Your evaluation was updated.";

        //Act
        await _ratingFacadeSUT.SaveAsync(rating);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var ratingFromDb = await dbxAssert.Rating.SingleAsync(i => i.Id == rating.Id);
        DeepAssert.Equal(rating, RatingModelMapper.MapToDetailModel(ratingFromDb));
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
}