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
}