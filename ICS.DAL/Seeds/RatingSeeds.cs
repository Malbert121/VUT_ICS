using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class RatingSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(RatingSeeds.Rating1);
            modelBuilder.Entity<RatingEntity>().HasData(RatingSeeds.Rating2);
        }
    }

    public class RatingSeeds
    {
        public static RatingEntity Rating1 = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"), //there is no Id of activity and student
            Points = 5,
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

        public static void Seed(ModelBuilder modelBuilder)
        {
            RatingSeedExtensions.Seed(modelBuilder);
        }
    }
}


