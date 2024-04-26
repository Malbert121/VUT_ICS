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
using Microsoft.EntityFrameworkCore.Storage;
using ICS.DAL.Entities;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class StudentFacadeTests : FacadeTestsBase
{
    private readonly IStudentFacade _studentFacadeSUT;
    private readonly StudentFacade _studentApliedFacadeSUT;

    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
        _studentApliedFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
    }
    



    [Fact]
    public async Task Create_WithNonExistingSubject_Throws()
    {
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            firstName = "Anna",
            lastName = "Tomson",
            photoURL = "http://www.example.com/index.html",
            subjects = new ObservableCollection<SubjectListModel>()
            {
                new SubjectListModel
                {
                    Id = Guid.NewGuid(),
                    name = "Database Systems",
                    abbreviation = "IDS"
                }
            }
        };

        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithoutSubject_EqualsCreated()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            firstName = "Barak",
            lastName = "Obama",
            photoURL = "http://www.example.com/index.html",
            subjects = new ObservableCollection<SubjectListModel>()
        };

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithExistingSubject_Throws()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            firstName = "Barak",
            lastName = "Obama",
            photoURL = "http://www.example.com/index.html",
            subjects = new ObservableCollection<SubjectListModel>()
            {
                new ()
                {
                    Id = SubjectSeeds.SubjectWithNoStudent.Id,
                    name = SubjectSeeds.SubjectWithNoStudent.Name,
                    abbreviation = SubjectSeeds.SubjectWithNoStudent.Abbreviation
                }
            },
        };

        //Act && Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Create_WithExistingAndNonExistingSubject_Throws()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            firstName = "Barak",
            lastName = "Obama",
            photoURL = "http://www.example.com/index.html",
            subjects = new ObservableCollection<SubjectListModel>()
            {
                new ()
                {
                    Id = SubjectSeeds.SubjectWithNoStudent.Id,
                    name = SubjectSeeds.SubjectWithNoStudent.Name,
                    abbreviation = SubjectSeeds.SubjectWithNoStudent.Abbreviation
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    name = "Database Systems",
                    abbreviation = "IDS"
                }
            },
        };

        //Act && Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.SaveAsync(model));
    }
 

    [Fact]
    public async Task GetAll_FromSeeded_ContainsSeeded()
    {
        //Arrange
        var listModel = StudentModelMapper.MapToListModel(StudentSeeds.Harry);

        //Act
        var returnedModel = await _studentFacadeSUT.GetAsync();

        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithNoSubjects);

        //Act
        var returnedModel = await _studentFacadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }


    [Fact]
    public async Task Update_WithoutSubject_EqualsUpdated()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.Harry);
        model.firstName = "Harry";
        model.lastName = "Potter";
        model.photoURL = "http://www.example.com/index.html";
        model.subjects = new ObservableCollection<SubjectListModel>();

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }


    [Fact]
    public async Task Update_ExistingStudent_UpdateSuccessfully()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentUpdate);
        model.firstName = "Updated";

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeede_DoesNotThrow()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithNoSubjects);
        model.firstName = "Updated";

        //Act & Assert
        await _studentFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task Update_RemoveSubject_EqualsUpdated()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithSubjects);
        model.subjects = new ObservableCollection<SubjectListModel>();

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveNonExistingSubject_Throws()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithSubjects);
        model.subjects = new ObservableCollection<SubjectListModel>
        {
            new SubjectListModel
            {
                Id = Guid.NewGuid(),
                name = "NonExisting",
                abbreviation = "NE"
            }
        };

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.SaveAsync(model));
    }


    [Fact]
    public async Task Delete_NonExisting_Throws()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentDelete);
        model.Id = Guid.NewGuid();

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.DeleteAsync(model.Id));
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _studentFacadeSUT.DeleteAsync(StudentSeeds.StudentDelete.Id);
    }

    [Fact]
    public async Task DeleteById_FromEmptySeeded_Throws()
    {
        //Arrange & Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.DeleteAsync(StudentSeeds.EmptyEntity.Id));
    }

    [Fact]
    public async Task Search_BySubstringLastName_DoesNotThrow()
    {
         var studentList = await _studentApliedFacadeSUT.GetSearchAsync("Potter");
         Assert.NotNull(studentList);
    }

    [Fact]
    public async Task Search_NonExistingSubject_ReturnsEmpty()
    {
        var studentList = await _studentApliedFacadeSUT.GetSearchAsync("NonExisting");
        Assert.Empty(studentList);
    }

    [Fact]
    public async Task Search_BySubstringFirstName_OneResult()
    {
        var studentList = await _studentApliedFacadeSUT.GetSearchAsync("Malfoy");
        Assert.Single(studentList.ToObservableCollection());
    }

    [Fact]
    public async Task Sort_ByDescendingId_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byDescendingId");
        Assert.Equal(Guid.Parse("F3A3E3A3-7B1A-48C1-9796-D2BAC7F67868"), studentList.ToObservableCollection()[0].Id);
        Assert.Equal(Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), studentList.ToObservableCollection()[5].Id);
    }

    [Fact]
    public async Task Sort_ByLastName_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byLastName");
        Assert.Equal(6, studentList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task Sort_ByDescendingLastName_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byDescendingLastName");
        Assert.Equal("Potter", studentList.ToObservableCollection()[0].lastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[1].lastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[2].lastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[3].lastName);
        Assert.Equal("Malfoy", studentList.ToObservableCollection()[4].lastName);
        Assert.Equal("Granger", studentList.ToObservableCollection()[5].lastName);

    }


    private static void FixIds(StudentDetailModel expectedModel, StudentDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var subjectModel in returnedModel.subjects)
        {
            var subjectDetailModel = expectedModel.subjects.FirstOrDefault(i =>
                i.name == subjectModel.name && i.abbreviation == subjectModel.abbreviation);


            if (subjectDetailModel != null)
            {
                subjectModel.Id = subjectDetailModel.Id;
                subjectModel.name = subjectDetailModel.name;
                subjectModel.abbreviation = subjectDetailModel.abbreviation;
            }
        }
    }
}