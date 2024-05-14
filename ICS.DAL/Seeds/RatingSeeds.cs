using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{

    public static class RatingSeeds
    {
        public static RatingEntity Rating1 = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
            Points = 7,
            Note = "Good job",
            Activity = ActivitySeeds.potionActivity,
            ActivityId = ActivitySeeds.potionActivity.Id,
            Student = StudentSeeds.student1,
            StudentId = StudentSeeds.student1.Id
        };

        public static RatingEntity Rating2 = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b56e8"),
            Points = 4,
            Note = "Good job",
            Activity = ActivitySeeds.potionActivity,
            ActivityId = ActivitySeeds.potionActivity.Id,
            Student = StudentSeeds.student2,
            StudentId = StudentSeeds.student2.Id
        };

        public static RatingEntity Rating3 = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f455"),
            Points = 8,
            Note = "Good job",
            Activity = ActivitySeeds.DefenceDarkArtsActivity,
            ActivityId = ActivitySeeds.DefenceDarkArtsActivity.Id,
            Student = StudentSeeds.student1,
            StudentId = StudentSeeds.student1.Id
        };

        public static RatingEntity Rating4 = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b5666"),
            Points = 5,
            Note = "Good job",
            Activity = ActivitySeeds.DefenceDarkArtsActivity,
            ActivityId = ActivitySeeds.DefenceDarkArtsActivity.Id,
            Student = StudentSeeds.student2,
            StudentId = StudentSeeds.student2.Id
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(Rating1 with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(Rating2 with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(Rating3 with { Activity = null!, Student = null! });
            modelBuilder.Entity<RatingEntity>().HasData(Rating4 with { Activity = null!, Student = null! });
        }
    }
}


