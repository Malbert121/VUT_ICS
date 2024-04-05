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
using Microsoft.EntityFrameworkCore.Storage;

namespace ICS.BL.Tests;

[Collection("SQLite Tests")]
public sealed class StudentFacadeTests : FacadeTestsBase, IAsyncLifetime
{
    private readonly IStudentFacade _studentFacadeSUT;

    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
    }
    



    [Fact]
    public async Task Create_WithNonExistingSubject_Throws()
    {
        var model = new StudentDetailModel()
        {
            Id = Guid.Empty,
            firstName = "Anna",
            lastName = "Tomson",
            fotoURL = "http://www.example.com/index.html",
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
            fotoURL = "http://www.example.com/index.html",
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
            fotoURL = "http://www.example.com/index.html",
            subjects = new ObservableCollection<SubjectListModel>()
            {
                new ()
                {
                    Id = SubjectSeeds.SubjectWithNoStudent.Id,
                    name = SubjectSeeds.SubjectWithNoStudent.name,
                    abbreviation = SubjectSeeds.SubjectWithNoStudent.abbreviation
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
        model.fotoURL = "http://www.example.com/index.html";
        model.subjects = new ObservableCollection<SubjectListModel>();

        //Act
        var returnedModel = await _studentFacadeSUT.SaveAsync(model);

        //Assert
        FixIds(model, returnedModel);
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrou()
    {
        //Arrange
        var model = StudentModelMapper.MapToDetailModel(StudentSeeds.Harry);
        model.firstName = "Changed";

        //Act && Assert
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