using ICS.DAL.Entities;

namespace ICS.DAL
{
    public class ActivityEntityHelper
    {
        private static int counter = 0;

        public static ActivityEntity CreateRandomActivity(SubjectEntity subject)
        {
            counter++;
            return new ActivityEntity
            {
                Id = Guid.NewGuid(),
                Name = $"name{counter}",
                Subject = subject,
                SubjectId = subject.Id,
                Room = $"room{counter}",
                ActivityTypeTag = $"tag{counter}",
                Description = "Default"
            };
        }
    }
}
