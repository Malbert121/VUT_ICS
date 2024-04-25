using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class SubjectSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(SubjectSeeds.potions);
        }
    }

    public class SubjectSeeds
    {
        public static SubjectEntity potions = new SubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140cbab73f4"),
            Name = "Potions",
            Abbreviation = "POT",
        };

        static SubjectSeeds()
        {
            potions.Students.Add(StudentSeeds.student1);
            potions.Students.Add(StudentSeeds.student2);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            SubjectSeedExtensions.Seed(modelBuilder);
        }
    }
}