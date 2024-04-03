using System;
using ICS.DAL.Entities;

namespace ICS.Common.Tests.Seeds
{
	public static class ActivitySeeds
	{
		public static readonly ActivityEntity EmptyActivity = new ActivityEntity
		{
			Id = Guid.Empty,
			name = string.Empty,
			start = DateTime.MinValue,
			end = DateTime.MinValue,
			room = string.Empty,
			activityTypeTag = string.Empty,
			description = string.Empty,
			subject = SubjectSeeds.EmptySubject,
			ratings = Array.Empty<RatingEntity>()
		};

		public static readonly ActivityEntity PotionsActivity = new ActivityEntity
		{
			Id = Guid.Parse("12b98f97-30de-4df2-8c33-bef54679f485"),
			name = "Potions lecture",
			start = new DateTime(2021, 10, 10, 10, 0, 0),
			end = new DateTime(2021, 10, 10, 12, 0, 0),
			room = "A03",
			activityTypeTag = "POT",
			description = "Brewing a potion",
			subject = SubjectSeeds.potions

		};

		public static readonly ActivityEntity ActivityWithNoRatings = PotionsActivity with { Id = Guid.Parse("5F6A8D0C-9B3E-4FD7-BD4A-1C9F20E6BDA7"), ratings = Array.Empty<RatingEntity>() };
		public static readonly ActivityEntity ActivityWithOneRating = PotionsActivity with { Id = Guid.Parse("E7F9E9C6-D29A-4B16-9D8C-9B21C5B78C4F"), ratings = new List<RatingEntity>() };
		public static readonly ActivityEntity ActivityWithTwoRatings = PotionsActivity with { Id = Guid.Parse("2A6BFC68-1BC0-40F9-BF0D-16AF3B7E9B86"), ratings = new List<RatingEntity>() };
		public static readonly ActivityEntity ActivityUpdate = PotionsActivity with { Id = Guid.Parse("7D5DE8AB-3E62-4F17-BE1D-0FC2A892B5F3"), ratings = Array.Empty<RatingEntity>() };
		public static readonly ActivityEntity ActivityDelete = PotionsActivity with { Id = Guid.Parse("C8D6A2E3-1DE4-4375-BFD1-86B3A1E8FC29"), ratings = Array.Empty<RatingEntity>() };

		static ActivitySeeds()
		{
			ActivityWithOneRating.ratings.Add(RatingSeeds.Rating1);
			ActivityWithTwoRatings.ratings.Add(RatingSeeds.Rating1);
			ActivityWithTwoRatings.ratings.Add(RatingSeeds.Rating2);
		}

		public static void Seed(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ActivityEntity>().HasData(PotionsActivity, ActivityWithNoRatings, ActivityWithOneRating, ActivityWithTwoRatings, ActivityUpdate, ActivityDelete);
		}
	}
}

