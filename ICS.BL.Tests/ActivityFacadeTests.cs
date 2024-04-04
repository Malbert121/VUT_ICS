﻿using ICS.DAL;
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

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class ActivityFacadeTests : FacadeTestsBase, IAsyncLifetime
{
    private readonly IActivityFacade _activityFacadeSUT;
    private SchoolContext _context;
    private IDbContextTransaction _transaction;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }
    
    public async Task InitializeAsync()
    {
        // Инициализация контекста и начало транзакции
        var options = DbContextOptionsConfigurer.ConfigureSqliteOptions(); 
        _context = new SchoolContext(options); // Укажите параметры для вашего контекста
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await _context.DisposeAsync();
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
}