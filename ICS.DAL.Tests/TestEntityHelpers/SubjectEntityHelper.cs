using ICS.DAL.Entities;

namespace ICS.DAL
{
    public class SubjectEntityHelper
    {
        private static int counter = 0;

        public static SubjectEntity CreateRandomSubject()
        {
            counter++;
            return new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = $"name{counter}",
                Abbreviation = $"abbreviation{counter}",
            };
        }
    }
}
