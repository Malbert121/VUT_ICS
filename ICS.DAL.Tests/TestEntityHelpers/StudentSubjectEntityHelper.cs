using ICS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.DAL.Tests.TestEntityHelpers
{
    public class StudentSubjectEntityHelper
    {
        private static int counter = 0;

        public static StudentSubjectEntity CreateRandomStudent(StudentEntity student, SubjectEntity subject)
        {
            counter++;
            return new StudentSubjectEntity
            {
                Id = Guid.NewGuid(),
                Student = student,
                Subject = subject,
                StudentId = student.Id,
                SubjectId = subject.Id
            };
        }
    }
}
