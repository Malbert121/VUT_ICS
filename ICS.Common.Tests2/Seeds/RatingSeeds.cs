using System;
using ICS.DAL.Entities;
using ICS.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests.Seeds
{
    public static class RatingSeeds
    {
        public static readonly RatingEntity EmptyEntity = new RatingEntity
        {
            Id = Guid.Empty,
            Points = 0,
            Note = string.Empty,
            Activity = ActivitySeeds.EmptyActivity,
            ActivityId = ActivitySeeds.EmptyActivity.Id,
            Student = StudentSeeds.EmptyEntity,
            StudentId = StudentSeeds.EmptyEntity.Id
        };

        public static readonly RatingEntity Rating1 = new RatingEntity
        {
            Id = Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            Points = 4,
            Note = "Average",
            Activity = ActivitySeeds.PotionsActivity,
            ActivityId = ActivitySeeds.PotionsActivity.Id,
            Student = StudentSeeds.Harry,
            StudentId = StudentSeeds.Harry.Id
        };

        public static readonly RatingEntity Rating2 = new RatingEntity
        {
            Id = Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
            Points = 5,
            Note = "Good",
            Activity = ActivitySeeds.PotionsActivity,
            ActivityId = ActivitySeeds.PotionsActivity.Id,
            Student = StudentSeeds.Hermione,
            StudentId = StudentSeeds.Hermione.Id
        };

        public static readonly RatingEntity RatingUpdate = new RatingEntity
        {
            Id = Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"),
            Points = 10,
            Note = "Good",
            Activity = ActivitySeeds.ActivityWithTwoRatings,
            ActivityId = ActivitySeeds.ActivityWithTwoRatings.Id,
            Student = StudentSeeds.Hermione,
            StudentId = StudentSeeds.Hermione.Id
        };

        public static readonly RatingEntity RatingDelete = new RatingEntity
        {
            Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"),
            Points = 5,
            Note = "Good",
            Activity = ActivitySeeds.ActivityWithTwoRatings,
            ActivityId = ActivitySeeds.ActivityWithTwoRatings.Id,
            Student = StudentSeeds.Hermione,
            StudentId = StudentSeeds.Hermione.Id
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(Rating1 with { Activity = null!, Student = null! },
                                                        Rating2 with { Activity = null!, Student = null! }, 
                                                        RatingUpdate with {Activity = null!, Student = null! },
                                                        RatingDelete with { Activity = null!, Student = null! });
        }

    }
}


