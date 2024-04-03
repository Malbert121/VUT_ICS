using System;
using ICS.DAL.Entities;
using ICS.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests2.Seeds
{
    public static class RatingSeeds
    {
        public static readonly RatingEntity EmptyEntity = new RatingEntity
        {
            Id = Guid.Empty,
            points = 0,
            note = string.Empty,
            activity = ActivitySeeds.EmptyActivity,
            student = StudentSeeds.Harry
        };

        public static readonly RatingEntity Rating1 = new RatingEntity
        {
            Id = Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            points = 4,
            note = "Average",
            activity = ActivitySeeds.PotionsActivity,
            student = StudentSeeds.Harry
        };

        public static readonly RatingEntity Rating2 = new RatingEntity
        {
            Id = Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
            points = 5,
            note = "Good",
            activity = ActivitySeeds.PotionsActivity,
            student = StudentSeeds.Hermione
        };

        public static readonly RatingEntity RatingUpdate = new RatingEntity
        {
            Id = Guid.Parse("f3a3e3a3-7b1a-48c1-9796-d2bac7f67868"),
            points = 5,
            note = "Good",
            activity = ActivitySeeds.PotionsActivity,
            student = StudentSeeds.Hermione
        };

        public static readonly RatingEntity RatingDelete = new RatingEntity
        {
            Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"),
            points = 5,
            note = "Good",
            activity = ActivitySeeds.PotionsActivity,
            student = StudentSeeds.Hermione
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(Rating1 with {activity = null!, student = null!},
                                                        Rating2 with {activity = null!, student = null! }, 
                                                        RatingUpdate with {activity = null!, student = null! }, 
                                                        RatingDelete with {activity = null!, student = null! });
        }
    }
}


