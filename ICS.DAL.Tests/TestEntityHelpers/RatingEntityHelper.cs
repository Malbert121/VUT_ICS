using ICS.DAL.Entities;

namespace ICS.DAL
{
    public class RatingEntityHelper
    {
        private static int counter = 0;
        private static readonly Random random = new Random();

        public static RatingEntity CreateRandomRating(ActivityEntity? activity = null, StudentEntity? student = null)
        {
            counter++;
            return new RatingEntity
            {
                Id = Guid.NewGuid(),
                points = random.Next(0, 101),
                note = $"note{counter}",
                activity = activity,
                activityId = activity.Id, 
                student = student,
                studentId = student.Id
            };
        }
    }
}
