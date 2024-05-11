using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{

    public static class StudentSeeds
    {
        public static readonly StudentEntity student1 = new StudentEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            FirstName = "Harry",
            LastName = "Potter",
            PhotoUrl = @"https://upload.wikimedia.org/wikipedia/en/d/d7/Harry_Potter_character_poster.jpg",
        };


        public static readonly StudentEntity student2 = new StudentEntity
        {
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"),
            FirstName = "Hermione",
            LastName = "Granger",
            PhotoUrl = @"https://upload.wikimedia.org/wikipedia/en/d/d3/Hermione_Granger_poster.jpg",
        };

        public static readonly StudentEntity student3 = new StudentEntity
        {
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"),
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
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"),
            FirstName = "Neville",
            LastName = "Longbottom",
            PhotoUrl = @"https://static.wikia.nocookie.net/harrypotter/images/4/41/Normal_promo_neville_plant.jpg/revision/latest?cb=20071228151053",
        };


        static StudentSeeds()
        {
            student1.Subjects.Add(StudentSubjectSeeds.HarryPotions);
            student1.Subjects.Add(StudentSubjectSeeds.HarryDarkArts);
            student2.Subjects.Add(StudentSubjectSeeds.HermionePotions);
            student2.Subjects.Add(StudentSubjectSeeds.HermioneDarkArts);
            student3.Subjects.Add(StudentSubjectSeeds.RonaldDarkArts);
            student4.Subjects.Add(StudentSubjectSeeds.LunaDarkArts);
            student5.Subjects.Add(StudentSubjectSeeds.NevilleDarkArts);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>()
                .HasData(student1 with { Subjects = Array.Empty<StudentSubjectEntity>()},
                         student2 with { Subjects = Array.Empty<StudentSubjectEntity>()},
                         student3 with { Subjects = Array.Empty<StudentSubjectEntity>()},
                         student4 with { Subjects = Array.Empty<StudentSubjectEntity>()},
                         student5 with { Subjects = Array.Empty<StudentSubjectEntity>()}
                         );
        }
    }
}


