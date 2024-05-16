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
using ICS.BL.Tests.TestingOptions;

namespace ICS.BL.Tests;

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
            FirstName = "Anna",
            LastName = "Tomson",
            PhotoURL = "http://www.example.com/index.html",
            Subjects = new ObservableCollection<StudentSubjectListModel>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    SubjectName = "Database Systems",
                    SubjectAbbreviation = "IDS",
                    StudentId = Guid.Empty,
                    SubjectId = Guid.NewGuid()
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
            FirstName = "Barak",
            LastName = "Obama",
            PhotoURL = "http://www.example.com/index.html",
            Subjects = new ObservableCollection<StudentSubjectListModel>()
            
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
            FirstName = "Barak",
            LastName = "Obama",
            PhotoURL = "http://www.example.com/index.html",
            Subjects = new ObservableCollection<StudentSubjectListModel>()
           {
                new()
                {
                    Id = Guid.NewGuid(),
                    SubjectName = "Database Systems",
                    SubjectAbbreviation = "IDS",
                    StudentId = Guid.Empty,
                    SubjectId = SubjectSeeds.SubjectWithNoStudent.Id
                }
            }
             
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
            FirstName = "Barak",
            LastName = "Obama",
            PhotoURL = "http://www.example.com/index.html",
            Subjects = new ObservableCollection<StudentSubjectListModel>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    SubjectName = SubjectSeeds.SubjectWithNoStudent.Name,
                    SubjectAbbreviation = SubjectSeeds.SubjectWithNoStudent.Abbreviation,
                    StudentId = Guid.Empty,
                    SubjectId = SubjectSeeds.SubjectWithNoStudent.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    SubjectName = "NonExisting",
                    SubjectAbbreviation = "NE",
                    StudentId = Guid.Empty,
                    SubjectId = Guid.NewGuid()
                }
            }
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
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.Harry);

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
        model.FirstName = "Harry";
        model.LastName = "Potter";
        model.PhotoURL = "http://www.example.com/index.html";
        model.Subjects = new ObservableCollection<StudentSubjectListModel>();

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
        model.FirstName = "Updated";

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
        model.FirstName = "Updated";

        //Act & Assert
        await _studentFacadeSUT.SaveAsync(model);
    }

    [Fact]
    public async Task Update_RemoveSubject_EqualsUpdated()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithSubjects);
        model.Subjects = new ObservableCollection<StudentSubjectListModel>();

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

   /* [Fact]
    public async Task Update_RemoveNonExistingSubject_Throws()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.StudentWithSubjects);
        model.subjects = new ObservableCollection<StudentSubjectListModel>();
        {
            new()
            {
                Id = Guid.NewGuid(),
                SubjectName = "NonExisting",
                SubjectAbbreviation = "NE",
                StudentId = Guid.Empty,
                SubjectId = Guid.NewGuid()
            }
        };

        //Act & Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _studentFacadeSUT.SaveAsync(model));
    }*/


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
         var studentList = await _studentApliedFacadeSUT.GetSearchAsync("Harry Potter");
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
        var studentList = await _studentApliedFacadeSUT.GetSearchAsync("Draco Malfoy");
        Assert.Single(studentList.ToObservableCollection());
    }

    [Fact]
    public async Task Sort_ByDescendingId_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byDescendingId");
        Assert.Equal(Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14743cf"), studentList.ToObservableCollection()[0].Id);
        Assert.Equal(Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"), studentList.ToObservableCollection()[1].Id);
    }

    [Fact]
    public async Task Sort_ByLastName_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byLastName");
        Assert.Equal(9, studentList.ToObservableCollection().Count);
    }

    [Fact]
    public async Task Sort_ByDescendingLastName_DoesNotThrow()
    {
        var studentList = await _studentApliedFacadeSUT.GetSortedAsync("byDescendingLastName");
        Assert.Equal("Weasley", studentList.ToObservableCollection()[0].LastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[1].LastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[2].LastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[3].LastName);
        Assert.Equal("Potter", studentList.ToObservableCollection()[4].LastName);
        Assert.Equal("Malfoy", studentList.ToObservableCollection()[5].LastName);
        Assert.Equal("Lovegood", studentList.ToObservableCollection()[6].LastName);
        Assert.Equal("Longbottom", studentList.ToObservableCollection()[7].LastName);
        Assert.Equal("Granger", studentList.ToObservableCollection()[8].LastName);

    }


    private static void FixIds(StudentDetailModel expectedModel, StudentDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var studentSubjectModel in returnedModel.Subjects)
        {
            var StudentSubjectDetailModel = expectedModel.Subjects.FirstOrDefault(i =>
                i.SubjectName == studentSubjectModel.SubjectName && i.SubjectAbbreviation == studentSubjectModel.SubjectAbbreviation);


            if (StudentSubjectDetailModel != null)
            {
                studentSubjectModel.SubjectId = StudentSubjectDetailModel.Id;
                studentSubjectModel.SubjectName = StudentSubjectDetailModel.SubjectName;
                studentSubjectModel.SubjectAbbreviation = StudentSubjectDetailModel.SubjectAbbreviation;
            }
        }
    }
}