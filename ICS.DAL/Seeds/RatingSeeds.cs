using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class RatingSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(RatingSeeds.rating1);
            modelBuilder.Entity<RatingEntity>().HasData(RatingSeeds.rating2);
        }
    }

    public class RatingSeeds
    {
        public static RatingEntity rating1 = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"), //there is no Id of activity and student
            points = 5,
            note = "Good job",
            activity = ActivitySeeds.potionActivity,
            activityId = ActivitySeeds.potionActivity.Id,
            student = StudentSeeds.student1,
            studentId = StudentSeeds.student1.Id
        };

        public static RatingEntity rating2 = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b56e8"),
            points = 4,
            note = "Good job",
            activity = ActivitySeeds.potionActivity,
            activityId = ActivitySeeds.potionActivity.Id,
            student = StudentSeeds.student2,
            studentId = StudentSeeds.student2.Id
        };
    }
}


