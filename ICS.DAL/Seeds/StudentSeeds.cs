using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class StudentSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(StudentSeeds.student1, StudentSeeds.student2);
        }
    }


    public static class StudentSeeds
    {
        public static readonly StudentEntity student1 = new StudentEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            firstName = "Harry",
            lastName = "Potter",
            fotoURL = "http://www.example.com/index.html"
        };


        public static readonly StudentEntity student2 = new StudentEntity
        {
            Id = Guid.Parse("f6b5fcf8-1a45-4135-b826-f6d4f14703cf"),
            firstName = "Hermione",
            lastName = "Granger",
            fotoURL = "http://www.example.com/index.html"
        };

        static StudentSeeds()
        {
            student1.subjects.Add(SubjectSeeds.potions);
            student2.subjects.Add(SubjectSeeds.potions);
        }

    }
}


