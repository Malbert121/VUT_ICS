using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using System.Data;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Internal;
using ICS.DAL;



namespace ICS.DAL.Tests
{
    public class DbContextStudentTests
    {
        [Fact]
        public async Task AddNew_Student_Persisted()
        {
            StudentEntity entity;
            using (var context = new SchoolContext())
            {
                // Arrange
                entity = new StudentEntity
                {
                    Id = Guid.Empty,
                    firstName = "Hermione",
                    lastName = "Granger",
                    fotoURL = "https://upload.wikimedia.org/wikipedia/en/d/d3/Hermione_Granger_poster.jpg",
                };

                // Act
                context.Students.Add(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext())
            {
                var student = await context.Students.SingleAsync(i => i.studentId == entity.studentId);
                Assert.NotNull(student);
                Assert.Equal(entity.studentId, student.studentId);
            }
        }

        [Fact]
        public async Task Delete_Student_Persisted()
        {
            StudentEntity entity;
            using (var context = new SchoolContext())
            {
                // Arrange
                entity = new StudentEntity
                {
                    Id = Guid.Empty,
                    firstName = "Harry",
                    lastName = "Potter",
                    fotoURL = "https://upload.wikimedia.org/wikipedia/en/d/d7/Harry_Potter_character_poster.jpg",
                };

                context.Students.Add(entity);
                context.SaveChanges();
            }

            // Act
            using (var context = new SchoolContext())
            {
                context.Students.Remove(await context.Students.FindAsync(entity.studentId));
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new SchoolContext())
            {
                Assert.False(await context.Students.AnyAsync(i => i.studentId == entity.studentId));
            }
        }

        [Fact]
        public async Task Update_Student_Persisted()
        {
            StudentEntity entity;
            using (var context = new SchoolContext())
            {
                // Arrange
                entity = new StudentEntity
                {
                    Id = Guid.Empty,
                    firstName = "Hermione",
                    lastName = "Granger",
                    fotoURL = "https://upload.wikimedia.org/wikipedia/en/d/d3/Hermione_Granger_poster.jpg",
                };

                context.Students.Add(entity);
                context.SaveChanges();
            }

            // Act
            using (var context = new SchoolContext())
            {
                var student = await context.Students.SingleAsync(i => i.studentId == entity.studentId);
                if (student != null)
                {
                    student.lastName = "Weasley";
                    await context.SaveChangesAsync();
                }
            }

            // Assert
            using (var context = new SchoolContext())
            {
                var student = await context.Students.SingleAsync(i => i.studentId == entity.studentId);
                Assert.NotNull(student);
                Assert.Equal(entity.studentId, student.studentId);
                Assert.Equal("Weasley", student.lastName);
            }
        }

        [Fact]
        public async Task AddNew_SubjectToStudent_Persisted()
        {
            int studentId;
            int subjectId;
            using (var context = new SchoolContext())
            {
                // Arrange
                var student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
                var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

                context.Students.Add(student);

                // Act
                student.subjects.Add(subject);
                context.SaveChanges();

                studentId = student.studentId;
                subjectId = subject.subjectId;
            }

            // Assert
            using (var context = new SchoolContext())
            {
                var student = await context.Students.Include(i => i.subjects).SingleAsync(i => i.studentId == studentId);
                var subject = await context.Subjects.FindAsync(subjectId);
                Assert.True(student.subjects.Contains(subject));
                Assert.True(subject.students.Contains(student));
            }
        }

        [Fact]
        public async Task GetAll_Students_HaveSubject()
        {
            int subject1Id;
            using (var context = new SchoolContext())
            {
                // Arrange
                var student1 = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
                var student2 = new StudentEntity { Id = Guid.Empty, firstName = "Andrew", lastName = "Linkoln", fotoURL = "http://www.example.com/index.html" };
                var student3 = new StudentEntity { Id = Guid.Empty, firstName = "Anna", lastName = "Smith", fotoURL = "http://www.example.com/index.html" };
                var subject1 = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };
                var subject2 = new SubjectEntity { Id = Guid.Empty, name = "Database Systems", abbreviation = "IDS" };

                context.Students.Add(student1);
                context.Students.Add(student2);
                context.Students.Add(student3);
                context.SaveChanges();

                student1.subjects.Add(subject1);
                student2.subjects.Add(subject2);
                student3.subjects.Add(subject1);
                context.SaveChanges();

                subject1Id = subject1.subjectId;
            }

            using (var context = new SchoolContext())
            {
                // Act
                var studentsWithSubject = await context.Students.Include(i => i.subjects).Where(i => i.subjects.Any(i => i.subjectId == subject1Id)).ToArrayAsync();

                // Assert
                Assert.Equal(2, studentsWithSubject.Length);

                var subject1 = await context.Subjects.FindAsync(subject1Id);
                foreach (var student in studentsWithSubject)
                {
                    Assert.True(student.subjects.Contains(subject1));
                }
            }
        }

        [Fact]
        public async Task Delete_SubjectFromStudent_Persisted()
        {
            int studentId;
            int subjectId;
            using (var context = new SchoolContext())
            {
                // Arrange
                var student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
                var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

                context.Students.Add(student);
                student.subjects.Add(subject);
                context.SaveChanges();

                //Act
                student.subjects.Remove(subject);
                await context.SaveChangesAsync();

                studentId = student.studentId;
                subjectId = subject.subjectId;
            }

            // Assert
            using (var context = new SchoolContext())
            {
                var student = await context.Students.Include(i => i.subjects).SingleAsync(i => i.studentId == studentId);
                var subject = await context.Subjects.Include(i => i.students).SingleAsync(i => i.subjectId == subjectId); ;
                Assert.False(student.subjects.Contains(subject));
                Assert.False(subject.students.Contains(student));
            }
        }

        [Fact]
        public async Task GetAll_Activities_OfAStudent()
        {
            StudentEntity student;
            using (var context = new SchoolContext())
            {
                // Arrange
                student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
                var subject1 = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };
                var subject2 = new SubjectEntity { Id = Guid.Empty, name = "Database Systems", abbreviation = "IDS" };

                context.Students.Add(student);
                student.subjects.Add(subject1);
                student.subjects.Add(subject2);
                context.SaveChanges();

                var activity1 = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject1, subjectId = subject1.subjectId, room = "D0207", activityTypeTag = "S", description = "Fundamentals seminar" };
                var activity2 = new ActivityEntity { Id = Guid.Empty, name = "Lecture", subject = subject1, subjectId = subject1.subjectId, room = "D105", activityTypeTag = "L", description = "3h lecture" };
                var activity3 = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject2, subjectId = subject2.subjectId, room = "D0206", activityTypeTag = "S", description = "Fundamentals seminar" };
                subject1.activity.Add(activity1);
                subject1.activity.Add(activity2);
                subject2.activity.Add(activity3);
                context.SaveChanges();
            }

            using (var context = new SchoolContext())
            {
                // Act
                var studentToSchedule = await context.Students.Include(i => i.subjects).ThenInclude(i => i.activity).FirstOrDefaultAsync(i => i.studentId == student.studentId);
                var activitiesOfStudent = studentToSchedule.subjects.SelectMany(i => i.activity).ToList();

                // Assert
                Assert.Equal(3, activitiesOfStudent.Count);

            }
        }

        [Fact]
        public async Task GetAll_Subjects_OfAStudent()
        {
            StudentEntity student;
            using (var context = new SchoolContext())
            {
                // Arrange
                student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
                var subject1 = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };
                var subject2 = new SubjectEntity { Id = Guid.Empty, name = "Database Systems", abbreviation = "IDS" };
                var subject3 = new SubjectEntity { Id = Guid.Empty, name = "Physical Education", abbreviation = "PE" };

                context.Students.Add(student);
                student.subjects.Add(subject1);
                student.subjects.Add(subject2);
                context.Subjects.Add(subject3);
                context.SaveChanges();
            }

            using (var context = new SchoolContext())
            {
                // Act
                var studentToSchedule = await context.Students.Include(i => i.subjects).FirstOrDefaultAsync(i => i.studentId == student.studentId);
                var subjectsOfStudent = studentToSchedule.subjects.ToList();

                // Assert
                Assert.Equal(2, subjectsOfStudent.Count);

                foreach (var subject in subjectsOfStudent)
                {
                    Assert.True(subject.students.Contains(studentToSchedule));
                }

            }
        }
    }
}