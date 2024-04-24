using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ICS.Common.Tests.Seeds
{
    public static class SubjectSeeds
    {
        public static SubjectEntity EmptySubject = new SubjectEntity
        {
            Id = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56239"),
            Name = string.Empty,
            Abbreviation = string.Empty,
        };

        public static SubjectEntity potions = new SubjectEntity
        {
            Id = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Name = "Potions",
            Abbreviation = "POT",
            Students = new List<StudentEntity>(),
            Activity = new List<ActivityEntity>()
        };

        public static readonly SubjectEntity SubjectWithNoStudent = new SubjectEntity
        {
            Id = Guid.Parse("6E6E0B29-20D6-4723-99D5-9140CBAB73F5"),
            Name = "SubjectWithNoStudent",
            Abbreviation = "SWNS",
            Students = new List<StudentEntity>()
        };

        public static readonly SubjectEntity SubjectWithOneStudent = new SubjectEntity
        {
            Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"),
            Name = "SubjectWithOneStudent",
            Abbreviation = "SWOS",
            Students = new List<StudentEntity> { StudentSeeds.Harry }
        };

        public static readonly SubjectEntity SubjectWithTwoStudents = new SubjectEntity
        {
            Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"),
            Name = "SubjectWithTwoStudents",
            Abbreviation = "SWTS",
            Students = new List<StudentEntity> { StudentSeeds.Harry, StudentSeeds.Hermione }
        };

        public static readonly SubjectEntity SubjectUpdate = new SubjectEntity
        {
            Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052"),
            Name = "SubjectUpdate",
            Abbreviation = "SU",
            Students = new List<StudentEntity>()
        };

        public static readonly SubjectEntity SubjectDelete = new SubjectEntity
        {
            Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619"),
            Name = "SubjectDelete",
            Abbreviation = "SD",
            Students = new List<StudentEntity>()
        };
        static SubjectSeeds()
        {
            potions.Students.Add(StudentSeeds.Harry);
            potions.Students.Add(StudentSeeds.Hermione);
            potions.Activity.Add(ActivitySeeds.PotionsActivity);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(potions with { Students = Array.Empty<StudentEntity>(), Activity = Array.Empty<ActivityEntity>() }, EmptySubject, SubjectWithNoStudent with { Students = Array.Empty<StudentEntity>() }, SubjectWithOneStudent with { Students = Array.Empty<StudentEntity>() }, SubjectWithTwoStudents with { Students = Array.Empty<StudentEntity>() }, SubjectUpdate with { Students = Array.Empty<StudentEntity>() }, SubjectDelete with { Students = Array.Empty<StudentEntity>() });
        }

    }
}
