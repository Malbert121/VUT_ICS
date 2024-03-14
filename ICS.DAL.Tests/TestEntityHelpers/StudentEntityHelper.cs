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
                firstName = $"Name{counter}",
                lastName = $"Surname{counter}",
                fotoURL = "http://www.example.com/index.html"
            };
        }
    }
}
