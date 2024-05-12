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

        static ActivitySeeds()
        {
            potionActivity.Ratings.Add(RatingSeeds.Rating1);
            potionActivity.Ratings.Add(RatingSeeds.Rating2);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>().HasData(
                potionActivity with { Subject = null!, Ratings = Array.Empty<RatingEntity>() });
        }
    }
}

