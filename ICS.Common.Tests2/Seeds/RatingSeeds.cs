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
            activity = ActivitySeeds.potionActivity,
            student = StudentSeeds.Harry
        };

        public static readonly RatingEntity Rating2 = new RatingEntity
        {
            Id = Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
            points = 5,
            note = "Good",
            activity = ActivitySeeds.potionActivity,
            student = StudentSeeds.Hermione
        };

        public static readonly RatingEntity RatingUpdate = Rating1 with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674") };
        public static readonly RatingEntity RatingDelete = Rating1 with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279") };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(Rating1, Rating2, RatingUpdate, RatingDelete);
        }
    }
}


