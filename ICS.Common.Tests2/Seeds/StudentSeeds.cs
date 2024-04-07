using System;
using System.Collections.Generic;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests2.Seeds
{
    public static class StudentSeeds
    {
        public static readonly StudentEntity EmptyEntity = new StudentEntity
        {
            Id = Guid.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            FotoUrl = string.Empty,
        };

        public static readonly StudentEntity Harry = new StudentEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            FirstName = "Harry",
            LastName = "Potter",
            FotoUrl = "http://www.example.com/index.html"
        };

        public static readonly StudentEntity Hermione = new StudentEntity
        {
            Id = Guid.Parse("df935095-8709-4040-a2bb-b6f97cb416dc"),
            FirstName = "Hermione",
            LastName = "Granger",
            FotoUrl = "http://www.example.com/index.html"
        };

        public static readonly StudentEntity StudentWithNoSubjects = new StudentEntity
        {
            Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"),
            FirstName = "Harry",
            LastName = "Potter",
            FotoUrl = "http://www.example.com/index.html",
            Subjects = new List<SubjectEntity>()
        };

        public static readonly StudentEntity StudentWithSubjects = new StudentEntity
        {
            Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"),
            FirstName = "Harry",
            LastName = "Potter",
            FotoUrl = "http://www.example.com/index.html",
            Subjects = new List<SubjectEntity> { SubjectSeeds.potions }
        };

        public static readonly StudentEntity StudentUpdate = new StudentEntity
        {
            Id = Guid.Parse("F3A3E3A3-7B1A-48C1-9796-D2BAC7F67868"),
            FirstName = "Harry",
            LastName = "Potter",
            FotoUrl = "http://www.example.com/index.html",
            Subjects = new List<SubjectEntity>()
        };

        public static readonly StudentEntity StudentDelete = new StudentEntity
        {
            Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"),
            FirstName = "Harry",
            LastName = "Potter",
            FotoUrl = "http://www.example.com/index.html",
            Subjects = new List<SubjectEntity>()
        };

        static StudentSeeds()
        {
            StudentWithSubjects.Subjects.Add(SubjectSeeds.potions);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(Harry with { Subjects = Array.Empty<SubjectEntity>() }, Hermione with { Subjects = Array.Empty<SubjectEntity>() }, StudentWithNoSubjects with {Subjects = Array.Empty<SubjectEntity>() }, 
                StudentWithSubjects with { Subjects = Array.Empty<SubjectEntity>() }, 
                StudentUpdate with { Subjects = Array.Empty<SubjectEntity>() }, 
                StudentDelete with { Subjects = Array.Empty<SubjectEntity>() });
        }
    }
}
