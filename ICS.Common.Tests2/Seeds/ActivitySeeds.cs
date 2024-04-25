using System;
using System.Reflection.Emit;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests.Seeds
{
    public static class ActivitySeeds
    {
        public static readonly ActivityEntity EmptyActivity = new ActivityEntity
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Start = DateTime.MinValue,
            End = DateTime.MinValue,
            Room = string.Empty,
            ActivityTypeTag = string.Empty,
            Description = string.Empty,
            SubjectId = SubjectSeeds.EmptySubject.Id,
            Subject = SubjectSeeds.EmptySubject,
            Ratings = Array.Empty<RatingEntity>()
        };

        public static readonly ActivityEntity PotionsActivity = new ActivityEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
        };

        public static readonly ActivityEntity ActivityWithNoRatings = new ActivityEntity
        {
            Id = Guid.Parse("5F6A8D0C-9B3E-4FD7-BD4A-1C9F20E6BDA7"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
            Ratings = Array.Empty<RatingEntity>()
        };

        public static readonly ActivityEntity ActivityWithOneRating = new ActivityEntity
        {
            Id = Guid.Parse("E7F9E9C6-D29A-4B16-9D8C-9B21C5B78C4F"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
            Ratings = new List<RatingEntity> ()
        };

        public static readonly ActivityEntity ActivityWithTwoRatings = new ActivityEntity
        {
            Id = Guid.Parse("2A6BFC68-1BC0-40F9-BF0D-16AF3B7E9B86"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
            Ratings = new List<RatingEntity> ()
        };

        public static readonly ActivityEntity ActivityUpdate = new ActivityEntity
        {
            Id = Guid.Parse("7D5DE8AB-3E62-4F17-BE1D-0FC2A892B5F3"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
            Ratings = Array.Empty<RatingEntity>()
        };

        public static readonly ActivityEntity ActivityDelete = new ActivityEntity
        {
            Id = Guid.Parse("D3D3D3D3-3D3D-3D3D-3D3D-3D3D3D3D3D3D"),
            Name = "Potions lecture",
            Start = new DateTime(2021, 10, 10, 10, 0, 0),
            End = new DateTime(2021, 10, 10, 12, 0, 0),
            Room = "A03",
            ActivityTypeTag = "POT",
            Description = "Brewing a potion",
            SubjectId = SubjectSeeds.potions.Id,
            Subject = SubjectSeeds.potions,
            Ratings = new List<RatingEntity>()
        };

       

        static ActivitySeeds()
        {
            PotionsActivity.Ratings.Add(RatingSeeds.Rating1);
            PotionsActivity.Ratings.Add(RatingSeeds.Rating2);
            ActivityWithTwoRatings.Ratings.Add(RatingSeeds.RatingUpdate);
            ActivityWithTwoRatings.Ratings.Add(RatingSeeds.RatingUpdate);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>().HasData(PotionsActivity with { Subject = null!, Ratings = Array.Empty<RatingEntity>() },
                ActivityWithNoRatings with { Subject = null!, Ratings = Array.Empty<RatingEntity>()}, 
                ActivityWithOneRating with { Subject = null!, Ratings = Array.Empty<RatingEntity>() }, 
                ActivityWithTwoRatings with { Subject = null!, Ratings = Array.Empty<RatingEntity>() }, 
                ActivityUpdate with { Subject = null!, Ratings = Array.Empty<RatingEntity>() }, 
                ActivityDelete with { Subject = null! });
        }
    }
}

