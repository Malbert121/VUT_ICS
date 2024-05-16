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
using ICS.Common.Tests.Seeds;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;
using ICS.BL.Tests.TestingOptions;

namespace ICS.BL.Tests;


public sealed class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSUT;
    
    private readonly SubjectFacade _subjectApliedFacadeSUT;


    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
        
        _subjectApliedFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
    }
    
    [Fact]
    public async Task Create_WithNonExistingStudent_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            Name = "Database Systems",
            Abbreviation = "IDS",
            Students = new ObservableCollection<StudentSubjectListModel>()
            {
                new StudentSubjectListModel
                {
                    Id = Guid.NewGuid(),
                    StudentFirstName = "Harry",
                    StudentLastName = "Potter",
                    StudentId = Guid.Empty,
                    SubjectId = SubjectSeeds.SubjectWithNoStudent.Id
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
            Name = "Database Systems",
            Abbreviation = "IDS",
            Activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = Guid.NewGuid(),
                    Name = "TESTACT",
                    Start = new DateTime(2021, 10, 10, 10, 0, 0),
                    End = new DateTime(2021, 10, 10, 10, 0, 0),
                    Room = "A03",
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
            Name = "Database Systems",
            Abbreviation = "IDS",
            Activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = Guid.NewGuid(),
                    Name = "TESTACT",
                    Start = new DateTime(2021, 10, 10, 10, 0, 0),
                    End = new DateTime(2021, 10, 10, 10, 0, 0),
                    Room = "A03",
                }
            },
            Students = new ObservableCollection<StudentSubjectListModel>()
            {
                new StudentSubjectListModel
                {
                    Id = Guid.NewGuid(),
                    StudentFirstName = "Hermione",
                    StudentLastName = "G",
                    StudentId = Guid.Empty,
                    SubjectId = SubjectSeeds.SubjectWithNoStudent.Id
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
            Name = "IMA3",
            Abbreviation = "Here you can study differential equations graciously."
        };

        await _subjectFacadeSUT.SaveAsync(model);
    }
    
    [Fact]
    public async Task Create_WithExistingActivity_Throws()
    {
        var model = new SubjectDetailModel()
        {
            Id = Guid.Empty,
            Name = "Database Systems",
            Abbreviation = "IDS",
            Activity = new ObservableCollection<ActivityListModel>()
            {
                new ActivityListModel
                {
                    Id = ActivitySeeds.PotionsActivity.Id,
                    Name = ActivitySeeds.PotionsActivity.Name,
                    Start = ActivitySeeds.PotionsActivity.Start,
                    End = ActivitySeeds.PotionsActivity.End,
                    Room = ActivitySeeds.PotionsActivity.Room,
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
            Name = "Database Systems",
            Abbreviation = "IDS",
            Students = new ObservableCollection<StudentSubjectListModel>()
            {
                new StudentSubjectListModel
                {
                    Id = Guid.NewGuid(),
                    StudentFirstName = "Harry",
                    StudentLastName = "Potter",
                    StudentId = Guid.Empty,
                    SubjectId = SubjectSeeds.SubjectWithNoStudent.Id
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
    public async Task GetAll_FromSeeded_ContainsSeeded()
    {
        //Arrange
        var listModel = SubjectModelMapper.MapToListModel(SubjectSeeds.potions);
    
        //Act
        var returnedModel = await _subjectFacadeSUT.GetAsync();
    
        //Assert
        Assert.Contains(listModel, returnedModel);
    }
    
    [Fact]
    public async Task Update_WithoutStudent_EqualsUpdated()
    {
        //Arrange
        var model = SubjectModelMapper.MapToDetailModel(SubjectSeeds.SubjectWithNoStudent);
        model.Name = "IMA5";
        model.Abbreviation = "Machine Learning prerequisites";

        //Act
        var returnedModel = await _subjectFacadeSUT.SaveAsync(model);

        //Assert
        FixStudentIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }
    
    [Fact]
    public async Task Update_ExistingSubject_UpdatesSuccessfully()
    {
        // Arrange
        var subject = new SubjectDetailModel { Name = "Initial Subject", Abbreviation = "INIT" };
        var createdSubject = await _subjectFacadeSUT.SaveAsync(subject);
        createdSubject.Name = "Updated Subject";
        createdSubject.Abbreviation = "UPDT";

        // Act
        var updatedSubject = await _subjectFacadeSUT.SaveAsync(createdSubject);

        // Assert
        FixActivityIds(createdSubject, updatedSubject);
        FixStudentIds(createdSubject, updatedSubject);
        DeepAssert.Equal(createdSubject, updatedSubject);
    }
    
    
    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var model = SubjectModelMapper.MapToDetailModel(SubjectSeeds.SubjectWithNoStudent);
        model.Name = "IMA4";

        //Act && Assert
        await _subjectFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.potions.Id);
    }
    
    [Fact]
    public async Task Delete_NonExisting_Throws()
    {
        //Arrange
        var model = SubjectModelMapper.MapToDetailModel(SubjectSeeds.SubjectDelete);
        model.Id = Guid.NewGuid();

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _subjectFacadeSUT.DeleteAsync(model.Id));
    }
    
    [Fact]
    public async Task SearchBySubstringName_NotThrow()
    {
        var subjectList = await _subjectApliedFacadeSUT.GetSearchAsync("Potions");

        Assert.Single(subjectList.ToObservableCollection());
    }
    
    [Fact]
    public async Task SearchNonExistingActivity_ReturnEmptyCollection()
    {
        var activityList = await _subjectApliedFacadeSUT.GetSearchAsync("IMA80");

        Assert.Equal(activityList.ToObservableCollection(), []);
    }
    
    [Fact]
    public async Task SortByDescendingId()
    {
        var activityList = await _subjectApliedFacadeSUT.GetSortedAsync("byDescendingId");

        Assert.Equal(7, activityList.ToObservableCollection().Count);
    }

    
    
    private static void FixActivityIds(SubjectDetailModel expectedModel, SubjectDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var activityModel in returnedModel.Activity)
        {
            var activityDetailModel = expectedModel.Activity.FirstOrDefault(i =>
                i.Name == activityModel.Name);


            if (activityDetailModel != null)
            {
                activityModel.Id = activityDetailModel.Id;
                activityModel.Name = activityDetailModel.Name;
                activityModel.Start = activityDetailModel.Start;
                activityModel.End = activityDetailModel.End;
                activityModel.Room = activityDetailModel.Room;
            }
        }
    }
    
    
    private static void FixStudentIds(SubjectDetailModel expectedModel, SubjectDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var studentModel in returnedModel.Students)
        {
            var studentDetailModel = expectedModel.Students.FirstOrDefault(i =>
                i.StudentFirstName == studentModel.StudentFirstName && i.StudentLastName == studentModel.StudentLastName);


            if (studentDetailModel != null)
            {
                studentModel.Id = studentDetailModel.Id;
                studentModel.StudentFirstName = studentModel.StudentFirstName;
                studentModel.StudentFirstName = studentModel.StudentLastName;
            }
        }
    }
}