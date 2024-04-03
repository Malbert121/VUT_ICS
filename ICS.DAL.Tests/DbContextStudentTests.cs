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
                entity.lastName = "Weasley";
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var student = await context.Students.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal("Weasley", student.lastName);
            }
        }

        [Fact]
        public async Task AddNew_SubjectToStudent_Persisted()
        {
            StudentEntity student;
            SubjectEntity subject;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                student = StudentEntityHelper.CreateRandomStudent();
                subject = SubjectEntityHelper.CreateRandomSubject();

                context.Students.Add(student);

                // Act
                student.subjects.Add(subject);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var actualStudent = await context.Students.Include(i => i.subjects).SingleAsync(i => i.Id == student.Id);
                var actualSubject = await context.Subjects.SingleAsync(i => i.Id == subject.Id);
                Assert.True(actualStudent.subjects.Contains(actualSubject));
                Assert.True(actualSubject.students.Contains(actualStudent));
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

                context.Students.Add(student1);
                context.Students.Add(student2);
                context.Students.Add(student3);

                student1.subjects.Add(subject1);
                student2.subjects.Add(subject2);
                student3.subjects.Add(subject1);
                context.SaveChanges();

                // Act
                var studentsWithSubject = await context.Students.Include(i => i.subjects).Where(i => i.subjects.Any(i => i.Id == subject1.Id)).ToArrayAsync();

                // Assert
                Assert.Equal(2, studentsWithSubject.Length);

                foreach (var student in studentsWithSubject)
                {
                    Assert.True(student.subjects.Contains(subject1));
                }
            }
        }

        [Fact]
        public async Task Delete_SubjectFromStudent_Persisted()
        {
            StudentEntity student;
            SubjectEntity subject;
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                student = StudentEntityHelper.CreateRandomStudent();
                subject = SubjectEntityHelper.CreateRandomSubject();

                context.Students.Add(student);
                student.subjects.Add(subject);
                context.SaveChanges();

                //Act
                student.subjects.Remove(subject);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var actualStudent = await context.Students.Include(i => i.subjects).SingleAsync(i => i.Id == student.Id);
                var actualSubject = await context.Subjects.Include(i => i.students).SingleAsync(i => i.Id == subject.Id); ;
                Assert.False(actualStudent.subjects.Contains(actualSubject));
                Assert.False(actualSubject.students.Contains(actualStudent));
            }
        }

        [Fact]
        public async Task GetAll_Activities_OfAStudent()
        {
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                var student = StudentEntityHelper.CreateRandomStudent();
                var subject1 = SubjectEntityHelper.CreateRandomSubject();
                var subject2 = SubjectEntityHelper.CreateRandomSubject();
                context.Students.Add(student);
                student.subjects.Add(subject1);
                student.subjects.Add(subject2);

                var activity1 = ActivityEntityHelper.CreateRandomActivity(subject1);
                var activity2 = ActivityEntityHelper.CreateRandomActivity(subject1);
                var activity3 = ActivityEntityHelper.CreateRandomActivity(subject2);
                subject1.activity.Add(activity1);
                subject1.activity.Add(activity2);
                subject2.activity.Add(activity3);
                context.SaveChanges();

                // Act
                var activitiesOfAStudent = student.subjects.SelectMany(i => i.activity).ToList();

                // Assert
                Assert.Equal(3, activitiesOfAStudent.Count);

            }
        }

        [Fact]
        public async Task GetAll_Subjects_OfAStudent()
        {
            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
            using (var context = new SchoolContext(options))
            {
                // Arrange
                var student = StudentEntityHelper.CreateRandomStudent();
                var subject1 = SubjectEntityHelper.CreateRandomSubject();
                var subject2 = SubjectEntityHelper.CreateRandomSubject();
                var subject3 = SubjectEntityHelper.CreateRandomSubject();

                context.Students.Add(student);
                student.subjects.Add(subject1);
                student.subjects.Add(subject2);
                context.Subjects.Add(subject3);
                context.SaveChanges();

                // Act
                var subjectsOfStudent = student.subjects.ToList();

                // Assert
                Assert.Equal(2, subjectsOfStudent.Count);

                foreach (var subject in subjectsOfStudent)
                {
                    Assert.True(subject.students.Contains(student));
                }

            }
        }
    }
}