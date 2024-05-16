using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using System.Data;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Internal;
using ICS.DAL;
using ICS.DAL.Context;
using ICS.DAL.Entities;
using ICS.DAL.Tests.TestEntityHelpers;


namespace ICS.DAL.Tests
{
    public class DbContextStudentTests
    {
        [Fact]
        public async Task AddNew_Student_Persisted()
        {
            StudentEntity entity;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                entity = StudentEntityHelper.CreateRandomStudent();

                // Act
                context.Students.Add(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var student = await context.Students.SingleAsync(i => i.Id == entity.Id);
                Assert.NotNull(student);
                Assert.Equal(entity.Id, student.Id);
            }
        }

        [Fact]
        public async Task Delete_Student_Persisted()
        {
            StudentEntity entity;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                entity = StudentEntityHelper.CreateRandomStudent();
                context.Students.Add(entity);

                // Act
                context.Students.Remove(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                Assert.False(await context.Students.AnyAsync(i => i.Id == entity.Id));
            }
        }

        [Fact]
        public async Task Update_Student_Persisted()
        {
            StudentEntity entity;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                entity = StudentEntityHelper.CreateRandomStudent();
                context.Students.Add(entity);

                // Act
                entity.LastName = "Weasley";
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var student = await context.Students.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal("Weasley", student.LastName);
            }
        }

        [Fact]
        public async Task AddNew_SubjectToStudent_Persisted()
        {
            StudentEntity student;
            SubjectEntity subject;
            StudentSubjectEntity studentSubject;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                student = StudentEntityHelper.CreateRandomStudent();
                subject = SubjectEntityHelper.CreateRandomSubject();
                studentSubject = StudentSubjectEntityHelper.CreateStudentSubject(student, subject);

                context.StudentSubjects.Add(studentSubject);

                // Act
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var actualStudent = await context.Students
                    .Include(s => s.Subjects)
                    .ThenInclude(ss => ss.Subject)
                    .SingleOrDefaultAsync(s => s.Id == student.Id);

                var actualSubject = await context.Subjects
                    .Include(s => s.Students)
                    .ThenInclude(ss => ss.Student)
                    .SingleOrDefaultAsync(s => s.Id == subject.Id);

                Assert.NotNull(actualStudent);
                Assert.NotNull(actualSubject);
                Assert.Contains(actualStudent, actualSubject.Students.Select(ss => ss.Student));
                Assert.Contains(actualSubject, actualStudent.Subjects.Select(ss => ss.Subject));
            }
        }


        [Fact]
        public async Task GetAll_Students_HaveSubject()
        {
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                var student1 = StudentEntityHelper.CreateRandomStudent();
                var student2 = StudentEntityHelper.CreateRandomStudent();
                var student3 = StudentEntityHelper.CreateRandomStudent();
                var subject1 = SubjectEntityHelper.CreateRandomSubject();
                var subject2 = SubjectEntityHelper.CreateRandomSubject();
                var studentSubject1 = StudentSubjectEntityHelper.CreateStudentSubject(student1, subject1);
                var studentSubject2 = StudentSubjectEntityHelper.CreateStudentSubject(student2, subject1);
                var studentSubject3 = StudentSubjectEntityHelper.CreateStudentSubject(student3, subject2);

                context.StudentSubjects.Add(studentSubject1);
                context.StudentSubjects.Add(studentSubject2);
                context.StudentSubjects.Add(studentSubject3);
                context.SaveChanges();

                // Act 
                var students = await context.StudentSubjects.Include(i => i.Student).Where(i => i.SubjectId == subject1.Id).Select(i => i.Student).ToListAsync();

                // Assert
                Assert.Equal(2, students.Count);
                Assert.Contains(students, i => i.Id == student1.Id);
            }
         
        }

        [Fact]
        public async Task Delete_SubjectFromStudent_Persisted()
        {
            StudentEntity student;
            SubjectEntity subject;
            StudentSubjectEntity studentSubject;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                student = StudentEntityHelper.CreateRandomStudent();
                subject = SubjectEntityHelper.CreateRandomSubject();
                studentSubject = StudentSubjectEntityHelper.CreateStudentSubject(student, subject);

                context.StudentSubjects.Add(studentSubject);
                context.SaveChanges(); 
                Assert.Contains(studentSubject, student.Subjects);

                // Act
                student.Subjects.Remove(studentSubject);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var actualStudent = await context.Students
                    .Include(s => s.Subjects)
                    .SingleOrDefaultAsync(s => s.Id == student.Id);

                var actualSubject = await context.Subjects
                    .Include(s => s.Students)
                    .SingleOrDefaultAsync(s => s.Id == subject.Id);

                Assert.NotNull(actualStudent);
                Assert.NotNull(actualSubject);
                Assert.DoesNotContain(studentSubject, actualStudent.Subjects);
                Assert.DoesNotContain(studentSubject, actualSubject.Students);
            }
        }

        [Fact]
        public async Task GetAll_Activities_OfAStudent()
        {
            // Arrange
            StudentEntity student;
            SubjectEntity subject1;
            SubjectEntity subject2;
            StudentSubjectEntity studentSubject1;
            StudentSubjectEntity studentSubject2;
            ActivityEntity activity1;
            ActivityEntity activity2;
            ActivityEntity activity3;

            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                student = StudentEntityHelper.CreateRandomStudent();
                subject1 = SubjectEntityHelper.CreateRandomSubject();
                subject2 = SubjectEntityHelper.CreateRandomSubject();
                studentSubject1 = StudentSubjectEntityHelper.CreateStudentSubject(student, subject1);
                studentSubject2 = StudentSubjectEntityHelper.CreateStudentSubject(student, subject2);
                context.StudentSubjects.Add(studentSubject1);
                context.StudentSubjects.Add(studentSubject2);

                activity1 = ActivityEntityHelper.CreateRandomActivity(subject1);
                activity2 = ActivityEntityHelper.CreateRandomActivity(subject1);
                activity3 = ActivityEntityHelper.CreateRandomActivity(subject2);
                subject1.Activity.Add(activity1);
                subject1.Activity.Add(activity2);
                subject2.Activity.Add(activity3);

                context.SaveChanges();
            }

            // Act
            List<ActivityEntity> activitiesOfAStudent;
            using (var context = new SchoolContext(options))
            {
                var studentFromDb = await context.Students
                    .Include(s => s.Subjects)
                    .ThenInclude(ss => ss.Subject)
                    .ThenInclude(s => s.Activity)
                    .SingleAsync(s => s.Id == student.Id);

                activitiesOfAStudent = studentFromDb.Subjects
                    .SelectMany(ss => ss.Subject.Activity)
                    .ToList();
            }

            // Assert
            Assert.Equal(3, activitiesOfAStudent.Count);
        }


        [Fact]
        public async Task GetAll_Subjects_OfAStudent()
        {
            // Arrange
            StudentEntity student;
            SubjectEntity subject1;
            SubjectEntity subject2;
            SubjectEntity subject3;
            StudentSubjectEntity studentSubject1;
            StudentSubjectEntity studentSubject2;
            StudentSubjectEntity studentSubject3;

            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                student = StudentEntityHelper.CreateRandomStudent();
                subject1 = SubjectEntityHelper.CreateRandomSubject();
                subject2 = SubjectEntityHelper.CreateRandomSubject();
                subject3 = SubjectEntityHelper.CreateRandomSubject();
                studentSubject1 = StudentSubjectEntityHelper.CreateStudentSubject(student, subject1);
                studentSubject2 = StudentSubjectEntityHelper.CreateStudentSubject(student, subject2);
                studentSubject3 = StudentSubjectEntityHelper.CreateStudentSubject(student, subject3);

                context.StudentSubjects.Add(studentSubject1);
                context.StudentSubjects.Add(studentSubject2);
                context.StudentSubjects.Add(studentSubject3);
                context.SaveChanges();
            }

            // Act
            List<SubjectEntity> subjectsOfStudent;
            using (var context = new SchoolContext(options))
            {
                var studentFromDb = await context.Students
                    .Include(s => s.Subjects)
                    .ThenInclude(ss => ss.Subject)
                    .SingleAsync(s => s.Id == student.Id);

                subjectsOfStudent = studentFromDb.Subjects
                    .Select(ss => ss.Subject)
                    .ToList();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var studentFromDb = await context.Students
                    .Include(s => s.Subjects)
                    .ThenInclude(ss => ss.Subject)
                    .SingleAsync(s => s.Id == student.Id);

                Assert.Equal(student.Id, studentFromDb.Id);
                Assert.Equal(3, subjectsOfStudent.Count);

                foreach (var subject in subjectsOfStudent)
                {
                    Assert.Contains(subject.Students, s => s.StudentId == student.Id);
                }
            }
        }

    }
}