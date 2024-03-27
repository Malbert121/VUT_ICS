using System;
using ICS.DAL.Entities;

namespace ICS.DAL.Seeds
{
    public class RatingSeeds
    {
        public static RatingEntity rating1 = new RatingEntity
        {
            Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"), //there is no Id of activity and student
            points = 5,
            note = "Good job",
            activity = ActivitySeeds.potionActivity,
            student = StudentSeeds.student1
        };

        public static RatingEntity rating2 = new RatingEntity
        {
            Id = Guid.Parse("c7a85db5-b2ad-4a17-9cd1-868d961b56e8"),
            points = 4,
            note = "Good job",
            activity = ActivitySeeds.potionActivity,
            student = StudentSeeds.student2
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RatingEntity>().HasData(rating1);
            modelBuilder.Entity<RatingEntity>().HasData(rating2);
        }
    }
}


