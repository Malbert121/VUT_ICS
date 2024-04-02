using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class ActivitySeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>().HasData(ActivitySeeds.potionActivity);
        }
    }

    public class ActivitySeeds
    {
        public static ActivityEntity potionActivity = new ActivityEntity //there is no Id of subject
        {
            Id = Guid.Parse("b1079d8c-26e7-49b5-bdd5-f63b1d3d8598"),
            name = "Brewing a potion",
            start = new DateTime(2021, 10, 10, 10, 0, 0),
            end = new DateTime(2021, 10, 10, 12, 0, 0),
            room = "A03",
            activityTypeTag = "POT",
            description = "Brewing a potion",
            subject = SubjectSeeds.potions
        };

        static ActivitySeeds()
        {
            potionActivity.ratings.Add(RatingSeeds.rating1);
            potionActivity.ratings.Add(RatingSeeds.rating2);
        }

    }
}

