namespace ICS.DAL
{
    public class ActivityEntityHelper
    {
        private static int counter = 0;

        public static ActivityEntity CreateRandomActivity(SubjectEntity? subject = null)
        {
            counter++;
            return new ActivityEntity
            {
                Id = Guid.NewGuid(),
                name = $"name{counter}",
                subject = subject,
                subjectId = subject?.subjectId ?? 0, //TODO: fix
                room = $"room{counter}",
                activityTypeTag = $"tag{counter}",
                description = "Default"
            };
        }
    }
}
