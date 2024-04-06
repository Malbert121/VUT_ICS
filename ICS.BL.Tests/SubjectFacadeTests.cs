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
using ICS.DAL.Seeds;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace ICS.BL.Tests;


public sealed class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSUT;


    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
    }
    
    [Fact]
    public async Task Create_WithNonExistingStudent_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "Database Systems",
            abbreviation = "IDS",
            students = new ObservableCollection<StudentListModel>()
            {
                new StudentListModel
                {
                    Id = Guid.NewGuid(),
                    firstName = "Pavel",
                    lastName = "Maslov"
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task Create_WithNonExistingActivity_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "Database Systems",
            abbreviation = "IDS",
            activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = Guid.NewGuid(),
                    name = "TESTACT",
                    start = new DateTime(2021, 10, 10, 10, 0, 0),
                    end = new DateTime(2021, 10, 10, 10, 0, 0),
                    room = "A03",
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task Create_WithNonExistingActivityAndStudent_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "Database Systems",
            abbreviation = "IDS",
            activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = Guid.NewGuid(),
                    name = "TESTACT",
                    start = new DateTime(2021, 10, 10, 10, 0, 0),
                    end = new DateTime(2021, 10, 10, 10, 0, 0),
                    room = "A03",
                }
            },
            students = new ObservableCollection<StudentListModel>()
            {
                new StudentListModel
                {
                    Id = Guid.NewGuid(),
                    firstName = "Pavel",
                    lastName = "Maslov" 
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task Create_WithoutActivityAndStudent_EqualsCreated()
    {
        //Arrange
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "IMA3",
            abbreviation = "Here you can study differential equations graciously."
        };

        //Act
        var returnedModel = await _subjectFacadeSUT.SaveAsync(model);

        //Assert
        DeepAssert.Equal(model, returnedModel);
    }
    
    [Fact]
    public async Task Create_WithExistingActivity_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "Database Systems",
            abbreviation = "IDS",
            activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = ActivitySeeds.potionActivity.Id,
                    name = ActivitySeeds.potionActivity.name,
                    start = ActivitySeeds.potionActivity.start,
                    end = ActivitySeeds.potionActivity.end,
                    room = ActivitySeeds.potionActivity.room,
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task Create_WithExistingStudent_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            name = "Database Systems",
            abbreviation = "IDS",
            students = new ObservableCollection<StudentListModel>()
            {
                new StudentListModel
                {
                    Id = StudentSeeds.student1.Id,
                    firstName = StudentSeeds.student1.firstName,
                    lastName = StudentSeeds.student1.lastName,
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.SaveAsync(model));
    }
    
    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.potions);

        //Act
        var returnedModel = await _subjectFacadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }
    
    [Fact]
    public async Task Update_ExistingSubject_UpdatesSuccessfully()
    {
        // Arrange
        var subject = new SubjectDetailModel { name = "Initial Subject", abbreviation = "INIT" };
        var createdSubject = await _subjectFacadeSUT.SaveAsync(subject);
        createdSubject.name = "Updated Subject";
        createdSubject.abbreviation = "UPDT";

        // Act
        var updatedSubject = await _subjectFacadeSUT.SaveAsync(createdSubject);

        // Assert
        FixActivityIds(createdSubject, updatedSubject);
        FixStudentIds(createdSubject, updatedSubject);
        DeepAssert.Equal(createdSubject, updatedSubject);
    }
    
    [Fact]
    public async Task Update_FromSeeded_DoesNotThrou()
    {
        //Arrange
        var model = SubjectModelMapper.MapToDetailModel(SubjectSeeds.potions);
        model.name = "ChangedIMA4";

        //Act && Assert
        await _subjectFacadeSUT.SaveAsync(model);
    }
    
    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.potions.Id);
    }
    
    
    private static void FixActivityIds(SubjectDetailModel expectedModel, SubjectDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var activityModel in returnedModel.activity)
        {
            var activityDetailModel = expectedModel.activity.FirstOrDefault(i =>
                i.name == activityModel.name);


            if (activityDetailModel != null)
            {
                activityModel.Id = activityDetailModel.Id;
                activityModel.name = activityDetailModel.name;
                activityModel.start = activityDetailModel.start;
                activityModel.end = activityDetailModel.end;
                activityModel.room = activityDetailModel.room;
            }
        }
    }
    
    
    private static void FixStudentIds(SubjectDetailModel expectedModel, SubjectDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var studentModel in returnedModel.students)
        {
            var studentDetailModel = expectedModel.students.FirstOrDefault(i =>
                i.firstName == studentModel.firstName && i.lastName == studentModel.lastName);


            if (studentDetailModel != null)
            {
                studentModel.Id = studentDetailModel.Id;
                studentModel.firstName = studentModel.firstName;
                studentModel.lastName = studentModel.lastName;
            }
        }
    }




}