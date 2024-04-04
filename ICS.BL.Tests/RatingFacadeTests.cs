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

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class RatingFacadeTests : FacadeTestsBase, IAsyncLifetime
{
    private readonly IRatingFacade _ratingFacadeSUT;
    private SchoolContext _context;
    private IDbContextTransaction _transaction;

    public RatingFacadeTests(ITestOutputHelper output) : base(output)
    {
        _ratingFacadeSUT = new RatingFacade(UnitOfWorkFactory, RatingModelMapper);
    }
    
    public async Task InitializeAsync()
    {
        var options = DbContextOptionsConfigurer.ConfigureSqliteOptions(); 
        _context = new SchoolContext(options); 
        await _context.Database.EnsureCreatedAsync();
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await _context.DisposeAsync();
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
    public async Task AddTestDataAsync()
    {
        var ratingEntity = new RatingEntity
        {
            Id = Guid.NewGuid(), 
            points = 20, 
            note = "note",
            activityId = Guid.NewGuid(), 
            studentId = Guid.NewGuid(),
            student = new StudentEntity
            {
                Id = Guid.NewGuid(),
            }
        };

        _context.Rating.Add(ratingEntity);
        await _context.SaveChangesAsync();
    }

        [Fact]
    public async Task GetAll_Single_SeededRating1()
    {
        await AddTestDataAsync();
        //Act
        var ratings = await _ratingFacadeSUT.GetAsync();
        var rating = ratings.Single(i => i.Id == RatingSeeds.Rating1.Id);
        //Assert
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
        var rating = await _ratingFacadeSUT.GetAsync(RatingSeeds.EmptyEntity.Id);

        Assert.Null(rating);
    }

}