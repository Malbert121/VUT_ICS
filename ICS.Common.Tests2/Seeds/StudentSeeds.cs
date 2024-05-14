using System;
using System.Collections.Generic;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests.Seeds
{
    public static class StudentSeeds
    {
        public static readonly StudentEntity EmptyEntity = new StudentEntity
        {
            Id = Guid.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            PhotoUrl = string.Empty,
            Subjects = Array.Empty<StudentSubjectEntity>()
        };

        public static readonly StudentEntity Harry = new StudentEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            FirstName = "Harry",
            LastName = "Potter",
            PhotoUrl = "http://www.example.com/index.html",
        };

        public static readonly StudentEntity Hermione = new StudentEntity
        {
            Id = Guid.Parse("df935095-8709-4040-a2bb-b6f97cb416dc"),
            FirstName = "Hermione",
            LastName = "Granger",
            PhotoUrl = "http://www.example.com/index.html",
        };

        public static readonly StudentEntity StudentWithNoSubjects = new StudentEntity
        {
            Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"),
            FirstName = "Harry",
            LastName = "Potter",
            PhotoUrl = "http://www.example.com/index.html",
        };

        public static readonly StudentEntity StudentWithSubjects = new StudentEntity
        {
            Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"),
            FirstName = "Harry",
            LastName = "Potter",
            PhotoUrl = "http://www.example.com/index.html",
        };

        public static readonly StudentEntity StudentUpdate = new StudentEntity
        {
            Id = Guid.Parse("F3A3E3A3-7B1A-48C1-9796-D2BAC7F67868"),
            FirstName = "Draco",
            LastName = "Malfoy",
            PhotoUrl = "http://www.example.com/index.html",
        };

        public static readonly StudentEntity StudentDelete = new StudentEntity
        {
            Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"),
            FirstName = "Harry",
            LastName = "Potter",
        };

        public static readonly StudentEntity student3 = new StudentEntity
        {
            Id = Guid.Parse("F3A3E3A3-7B1A-48C1-9796-B2BAC7F67868"),
            FirstName = "Ronald",
            LastName = "Weasley",
            PhotoUrl = @"https://static.wikia.nocookie.net/harrypotter/images/8/85/Ron_Weasley.jpg/revision/latest?cb=20101104210200",
        };

        public static readonly StudentEntity student4 = new StudentEntity
        {
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"),
            FirstName = "Luna",
            LastName = "Lovegood",
            PhotoUrl = @"https://static.wikia.nocookie.net/harrypotter/images/e/ed/Luna_Lovegood.jpg/revision/latest/scale-to-width-down/1000?cb=20160902165706",
        };

        public static readonly StudentEntity student5 = new StudentEntity
        {
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14743cf"),
            FirstName = "Neville",
            LastName = "Longbottom",
            PhotoUrl = @"https://static.wikia.nocookie.net/harrypotter/images/4/41/Normal_promo_neville_plant.jpg/revision/latest?cb=20071228151053",
        };

        static StudentSeeds()
        {
            Harry.Subjects.Add(StudentSubjectSeeds.HarryPotions);
            Harry.Subjects.Add(StudentSubjectSeeds.HarryDarkArts);
            Harry.Subjects.Add(StudentSubjectSeeds.HarrySubjectWithTwoStudents);
            Hermione.Subjects.Add(StudentSubjectSeeds.HermionePotions);
            Hermione.Subjects.Add(StudentSubjectSeeds.HermioneDarkArts);
            student3.Subjects.Add(StudentSubjectSeeds.RonaldDarkArts);
            student4.Subjects.Add(StudentSubjectSeeds.LunaDarkArts);
            student5.Subjects.Add(StudentSubjectSeeds.NevilleDarkArts);
            
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(
                Harry with { Subjects = Array.Empty<StudentSubjectEntity>() },
                Hermione with { Subjects = Array.Empty<StudentSubjectEntity>() },
                StudentWithNoSubjects with { Subjects = Array.Empty<StudentSubjectEntity>() },
                StudentWithSubjects with { Subjects = Array.Empty<StudentSubjectEntity>() },
                StudentUpdate with { Subjects = Array.Empty<StudentSubjectEntity>() },
                StudentDelete with { Subjects = Array.Empty<StudentSubjectEntity>() },
                student3 with { Subjects = Array.Empty<StudentSubjectEntity>() },
                student4 with { Subjects = Array.Empty<StudentSubjectEntity>() },
                student5 with { Subjects = Array.Empty<StudentSubjectEntity>() }
                );
        }
    }
}
