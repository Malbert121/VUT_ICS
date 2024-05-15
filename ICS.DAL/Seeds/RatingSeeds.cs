using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{

    public static class RatingSeeds
    {
        public static RatingEntity RatingPotterPotions = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
            Points = 7,
            Note = "Good job",
            Activity = ActivitySeeds.potionActivity,
            ActivityId = ActivitySeeds.potionActivity.Id,
            Student = StudentSeeds.student1,
            StudentId = StudentSeeds.student1.Id
        };

        public static RatingEntity RatingHermionPotions = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b56e8"),
            Points = 8,
            Note = "Good job",
            Activity = ActivitySeeds.potionActivity,
            ActivityId = ActivitySeeds.potionActivity.Id,
            Student = StudentSeeds.student2,
            StudentId = StudentSeeds.student2.Id
        };

        public static RatingEntity RatingPotterPotionsExam = new RatingEntity
        {
            Id = Guid.Parse("a2c89c8d-0e3b-4d43-af6f-34455b6b125e"),
            Points = 5,
            Note = "Acceptable",
            Activity = ActivitySeeds.potionExam,
            ActivityId = ActivitySeeds.potionExam.Id,
            Student = StudentSeeds.student1,
            StudentId = StudentSeeds.student1.Id
        };

        public static RatingEntity RatingHermionePotionsExam = new RatingEntity
        {
            Id = Guid.Parse("e0d7e894-0d8c-4c54-aaef-06451e5044f9"),
            Points = 10,
            Note = "Outstanding",
            Activity = ActivitySeeds.potionExam,
            ActivityId = ActivitySeeds.potionExam.Id,
            Student = StudentSeeds.student2,
            StudentId = StudentSeeds.student2.Id
        };

        public static RatingEntity RatingPotterDark = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f455"),
            Points = 8,
            Note = "Good job",
            Activity = ActivitySeeds.DefenceDarkArtsActivity,
            ActivityId = ActivitySeeds.DefenceDarkArtsActivity.Id,
            Student = StudentSeeds.student1,
            StudentId = StudentSeeds.student1.Id
        };

        public static RatingEntity RatingHermionDark = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b5666"),
            Points = 5,
            Note = "Good job",
            Activity = ActivitySeeds.DefenceDarkArtsActivity,
            ActivityId = ActivitySeeds.DefenceDarkArtsActivity.Id,
            Student = StudentSeeds.student2,
            StudentId = StudentSeeds.student2.Id
        };

        public static RatingEntity LunaHerbologyLesson = new RatingEntity
        {
            Id = Guid.Parse("82d5fd80-2f63-40b6-b3a1-05609bde2a68"),
            Points = 7,
            Note = "Good job",
            Activity = ActivitySeeds.HerbologyLesson,
            ActivityId = ActivitySeeds.HerbologyLesson.Id,
            Student = StudentSeeds.student4,
            StudentId = StudentSeeds.student4.Id
        };

        public static RatingEntity NevilHerbologyLesson = new RatingEntity
        {
            Id = Guid.Parse("34415df3-ae75-4fb2-9b7b-1869fa74e857"),
            Points = 10,
            Note = "Outstanding",
            Activity = ActivitySeeds.HerbologyLesson,
            ActivityId = ActivitySeeds.HerbologyLesson.Id,
            Student = StudentSeeds.student5,
            StudentId = StudentSeeds.student5.Id
        };

        public static RatingEntity NevilHerbologyExam = new RatingEntity
        {
            Id = Guid.Parse("fa7944c0-0d67-45af-b151-91f388fd8305"),
            Points = 10,
            Note = "Outstanding",
            Activity = ActivitySeeds.HerbologyExam,
            ActivityId = ActivitySeeds.HerbologyExam.Id,
            Student = StudentSeeds.student5,
            StudentId = StudentSeeds.student5.Id
        };

        public static RatingEntity LunaHerbologyExam = new RatingEntity
        {
            Id = Guid.Parse("1e891d88-d8b9-4fe6-b123-61a9e2d69ef5"),
            Points = 3,
            Note = "Fail",
            Activity = ActivitySeeds.HerbologyExam,
            ActivityId = ActivitySeeds.HerbologyExam.Id,
            Student = StudentSeeds.student4,
            StudentId = StudentSeeds.student4.Id
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(RatingPotterPotions with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(RatingHermionPotions with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(RatingPotterDark with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(RatingHermionDark with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(RatingPotterPotionsExam with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(RatingHermionePotionsExam with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(LunaHerbologyLesson with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(NevilHerbologyLesson with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(NevilHerbologyExam with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(LunaHerbologyExam with { Activity = null!, Student = null! });

        }
    }
}


