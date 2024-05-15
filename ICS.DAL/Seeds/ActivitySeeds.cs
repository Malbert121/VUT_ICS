using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{

    public static class ActivitySeeds
    {
        public static ActivityEntity potionActivity = new ActivityEntity
        {
            Id = Guid.Parse("b1079d8c-26e7-49b5-bdd5-f63b1d3d8598"),
            Name = "Brewing a potion",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            Subject = SubjectSeeds.potions,
            SubjectId = SubjectSeeds.potions.Id,
            Ratings = new List<RatingEntity>()
        };

        public static ActivityEntity potionExam = new ActivityEntity
        {
            Id = Guid.Parse("34415df3-ae75-4fb2-9b7b-1869fa74e857"),
            Name = "Potion exam",
            Start = new DateTime(2022, 01, 16, 13, 0, 0),
            End = new DateTime(2022, 01, 16, 14, 0, 0),
            Room = "D105",
            ActivityTypeTag = "POTEX",
            Description = "Potion exam",
            Subject = SubjectSeeds.potions,
            SubjectId = SubjectSeeds.potions.Id,
            Ratings = new List<RatingEntity>()
        };

        public static ActivityEntity DefenceDarkArtsActivity = new ActivityEntity
        {
            Id = Guid.Parse("b1079d8c-26e7-49b5-bdd5-f63b1d3d8200"),
            Name = "Learning some spells",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A05",
            ActivityTypeTag = "POT",
            Description = "Learning some spells",
            Subject = SubjectSeeds.DefenceDarkArts,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Ratings = new List<RatingEntity>()
        };

        public static ActivityEntity DefenceDarkArtsExam = new ActivityEntity
        {
            Id = Guid.Parse("7efc1209-78e4-4a3f-8ba5-b6727d84d57a"),
            Name = "Dark Arts exam",
            Start = new DateTime(2022, 01, 15, 13, 0, 0),
            End = new DateTime(2022, 01, 15, 14, 0, 0),
            Room = "D105",
            ActivityTypeTag = "DDAEX",
            Description = "Dark Arts exam",
            Subject = SubjectSeeds.DefenceDarkArts,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Ratings = new List<RatingEntity>()
        };

        public static ActivityEntity HerbologyLesson = new ActivityEntity
        {
            Id = Guid.Parse("028aae21-f145-46f5-a04a-788c067c731d"),
            Name = "Herbology lesson",
            Start = new DateTime(2022, 03, 15, 13, 0, 0),
            End = new DateTime(2022, 03, 15, 14, 0, 0),
            Room = "M206",
            ActivityTypeTag = "HERL",
            Description = "Mandrake root care",
            Subject = SubjectSeeds.Herbology,
            SubjectId = SubjectSeeds.Herbology.Id,
            Ratings = new List<RatingEntity>()
        };

        public static ActivityEntity HerbologyExam = new ActivityEntity
        {
            Id = Guid.Parse("6eef0e95-1467-4e2b-bdb5-c60fc9823b78"),
            Name = "Herbology exam",
            Start = new DateTime(2022, 02, 15, 13, 0, 0),
            End = new DateTime(2022, 02, 15, 14, 0, 0),
            Room = "M206",
            ActivityTypeTag = "HEREX",
            Description = "Herbology exam",
            Subject = SubjectSeeds.Herbology,
            SubjectId = SubjectSeeds.Herbology.Id,
            Ratings = new List<RatingEntity>()
        };

        //static ActivitySeeds()
        //{
        //    potionActivity.Ratings.Add(RatingSeeds.RatingPotterPotions);
        //    potionActivity.Ratings.Add(RatingSeeds.RatingHermionPotions);
        //}

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>().HasData(
                potionActivity with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                potionExam with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                DefenceDarkArtsActivity with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                DefenceDarkArtsExam with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                HerbologyLesson with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                HerbologyExam with { Subject = null!, Ratings = Array.Empty<RatingEntity>() }
                );
        }
    }
}

