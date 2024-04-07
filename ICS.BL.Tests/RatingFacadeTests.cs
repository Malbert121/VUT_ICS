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
using ICS.DAL.Context;
using ICS.DAL.Entities;
using ICS.DAL.Mappers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class RatingFacadeTests : FacadeTestsBase, IAsyncLifetime
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
            activityId = Guid.Empty,
            studentId = Guid.Empty,
            activity = new ActivityEntity()
            {
                Id = Guid.Empty,
                name = "name",
                start = DateTime.MinValue,
                end = DateTime.MinValue,
                room = "room",
                subjectId = Guid.Empty,
                subject = new SubjectEntity()
                {
                    Id = Guid.NewGuid(),
                    name = "Database Systems",
                    abbreviation = "IDS"
                }
            },
            student = new StudentEntity()
            {
                Id = Guid.Empty,
                firstName = "John",
                lastName = "Doe",
                fotoURL = "http://www.example.com/index.html",
                subjects = new List<SubjectEntity>()
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
                room = "room",
                subjectId = Guid.Empty,
                subject = new SubjectEntity()
                {
                    Id = Guid.NewGuid(),
                    name = "Database Systems",
                    abbreviation = "IDS"
                }
            },
            student = new StudentEntity()
            {
                Id = Guid.Empty,
                firstName = "John",
                lastName = "Doe",
                fotoURL = "http://www.example.com/index.html",
                subjects = new List<SubjectEntity>()
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
            points = RatingSeeds.Rating1.points,
            note = RatingSeeds.Rating1.note,
            activityId = RatingSeeds.Rating1.activityId,
            studentId = RatingSeeds.Rating1.studentId,
            activity = RatingSeeds.Rating1.activity,
            student = RatingSeeds.Rating1.student
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
}