using ICS.DAL.Entities;

namespace ICS.DAL
{
    public class RatingEntityHelper
    {
        private static int counter = 0;
        private static readonly Random random = new Random();

        public static RatingEntity CreateRandomRating(ActivityEntity activity, StudentEntity student)
        {
            counter++;
            return new RatingEntity
            {
                Id = Guid.NewGuid(),
                Points = random.Next(0, 101),
                Note = $"note{counter}",
                Activity = activity,
                ActivityId = activity.Id, 
                Student = student,
                StudentId = student.Id
            };
        }
    }
}
