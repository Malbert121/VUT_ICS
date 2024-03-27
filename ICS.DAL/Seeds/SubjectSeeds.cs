using System;
using ICS.DAL.Entities;

namespace ICS.DAL.Seeds
{
    public class SubjectSeeds
    {
        public static SubjectEntity potions = new SubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140cbab73f4"),
            name = "Potions",
            abbreviation = "POT",

        };

        //add student1 and student2 to subject
        static SubjectSeeds()
        {
            potions.students.Add(StudentSeeds.student1);
            potions.students.Add(StudentSeeds.student2);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(potions);
        }
    }
}



