using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{


    public static class SubjectSeeds
    {
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

        static SubjectSeeds()
        {
            potions.Students.Add(StudentSubjectSeeds.HarryPotions);
            potions.Students.Add(StudentSubjectSeeds.HermionePotions);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.HarryDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.HermioneDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.RonaldDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.LunaDarkArts);
            DefenceDarkArts.Students.Add(StudentSubjectSeeds.NevilleDarkArts);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(
                SubjectSeeds.potions with { Activity = Array.Empty<ActivityEntity>(), Students = Array.Empty<StudentSubjectEntity>() },
                SubjectSeeds.DefenceDarkArts with { Activity = Array.Empty<ActivityEntity>(), Students = Array.Empty<StudentSubjectEntity>() });
        }
    }
}