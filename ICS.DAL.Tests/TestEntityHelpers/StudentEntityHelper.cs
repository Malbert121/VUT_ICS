using ICS.DAL.Entities;

namespace ICS.DAL
{
    public class StudentEntityHelper
    {
        private static int counter = 0;

        public static StudentEntity CreateRandomStudent()
        {
            counter++;
            return new StudentEntity
            {
                Id = Guid.NewGuid(),
                FirstName = $"Name{counter}",
                LastName = $"Surname{counter}",
                PhotoUrl = "http://www.example.com/index.html"
            };
        }
    }
}
