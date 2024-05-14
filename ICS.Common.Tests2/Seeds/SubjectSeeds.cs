using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ICS.Common.Tests.Seeds
{
    public static class SubjectSeeds
    {
        public static readonly SubjectEntity EmptySubject = new SubjectEntity
        {
            Id = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56239"),
            Name = string.Empty,
            Abbreviation = string.Empty,
            Activity = new List<ActivityEntity>()
        };

        public static SubjectEntity potions = new SubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140cbab73f4"),
            Name = "Potions",
            Abbreviation = "POT",
            Activity = new List<ActivityEntity>(),
            Students = new List<StudentSubjectEntity>()
        };

        public static SubjectEntity DefenceDarkArts = new SubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140444473f4"),
            Name = "Defence Against Dark Arts",
            Abbreviation = "DDA",
            Activity = new List<ActivityEntity>(),
            Students = new List<StudentSubjectEntity>()
        };

        public static readonly SubjectEntity SubjectWithNoStudent = new SubjectEntity
        {
            Id = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            Name = "SubjectWithNoStudent",
            Abbreviation = "SWNS",
            Activity = new List<ActivityEntity>()
        };

        public static readonly SubjectEntity SubjectWithOneStudent = new SubjectEntity
        {
            Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"),
            Name = "SubjectWithOneStudent",
            Abbreviation = "SWOS",
            Activity = new List<ActivityEntity>()
        };

        public static readonly SubjectEntity SubjectWithTwoStudents = new SubjectEntity
        {
            Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"),
            Name = "SubjectWithTwoStudents",
            Abbreviation = "SWTS",
            Activity = new List<ActivityEntity>()
        };

        public static readonly SubjectEntity SubjectUpdate = new SubjectEntity
        {
            Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052"),
            Name = "SubjectUpdate",
            Abbreviation = "SU",
            Activity = new List<ActivityEntity>()
        };

        public static readonly SubjectEntity SubjectDelete = new SubjectEntity
        {
            Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619"),
            Name = "SubjectDelete",
            Abbreviation = "SD",
            Activity = new List<ActivityEntity>()
        };

        static SubjectSeeds()
        {
            potions.Activity.Add(ActivitySeeds.PotionsActivity);
            potions.Activity.Add(ActivitySeeds.ActivityWithNoRatings);
            potions.Activity.Add(ActivitySeeds.ActivityWithOneRating);
            potions.Activity.Add(ActivitySeeds.ActivityWithTwoRatings);
            potions.Activity.Add(ActivitySeeds.ActivityUpdate);
            potions.Students.Add(StudentSubjectSeeds.HarryPotions);
            potions.Students.Add(StudentSubjectSeeds.HermionePotions);
            potions.Activity.Add(ActivitySeeds.ActivityDelete);
            SubjectWithTwoStudents.Students.Add(StudentSubjectSeeds.HarrySubjectWithTwoStudents);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.HarryDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.HermioneDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.RonaldDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.LunaDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.NevilleDarkArts);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(
                potions with { Students = Array.Empty<StudentSubjectEntity>(), 
                    Activity = Array.Empty<ActivityEntity>() }, 
                SubjectWithNoStudent with { Students = Array.Empty<StudentSubjectEntity>() }, 
                SubjectWithOneStudent with { Students = Array.Empty<StudentSubjectEntity>() }, 
                SubjectWithTwoStudents with { Students = Array.Empty<StudentSubjectEntity>(), Activity = Array.Empty<ActivityEntity>() }, 
                SubjectUpdate with { Students = Array.Empty<StudentSubjectEntity>() }, 
                SubjectDelete with { Students = Array.Empty<StudentSubjectEntity>() },
                DefenceDarkArts with { Students = Array.Empty<StudentSubjectEntity>() });
        }

    }
}
