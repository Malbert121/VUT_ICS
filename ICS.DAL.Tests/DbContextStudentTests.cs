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
                studentSubject = StudentSubjectEntityHelper.CreateRandomStudent(student, subject);

                context.StudentSubjects.Add(studentSubject);

                // Act
                student.Subjects.Add(studentSubject);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(options))
            {
                var actualStudent = await context.StudentSubjects.Include(i => i.Student).SingleAsync(i => i.Id == student.Id);
                var actualSubject = await context.StudentSubjects.SingleAsync(i => i.Id == subject.Id);
                Assert.True(actualStudent.Subject.Students.Contains(actualStudent));
                Assert.True(actualSubject.Student.Subjects.Contains(actualSubject));
            }
        }

        //[Fact]
        //public async Task GetAll_Students_HaveSubject()
        //{
        //    var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        //    using (var context = new SchoolContext(options))
        //    {
        //        // Arrange
        //        var student1 = StudentEntityHelper.CreateRandomStudent();
        //        var student2 = StudentEntityHelper.CreateRandomStudent();
        //        var student3 = StudentEntityHelper.CreateRandomStudent();
        //        var subject1 = SubjectEntityHelper.CreateRandomSubject();
        //        var subject2 = SubjectEntityHelper.CreateRandomSubject();

        //        context.Students.Add(student1);
        //        context.Students.Add(student2);
        //        context.Students.Add(student3);

        //        student1.Subjects.Add(subject1);
        //        student2.Subjects.Add(subject2);
        //        student3.Subjects.Add(subject1);
        //        context.SaveChanges();

        //        // Act
        //        var studentsWithSubject = await context.Students.Include(i => i.Subjects).Where(i => i.Subjects.Any(i => i.Id == subject1.Id)).ToArrayAsync();

        //        // Assert
        //        Assert.Equal(2, studentsWithSubject.Length);

        //        foreach (var student in studentsWithSubject)
        //        {
        //            Assert.True(student.Subjects.Contains(subject1));
        //        }
        //    }
        //}

        //[Fact]
        //public async Task Delete_SubjectFromStudent_Persisted()
        //{
        //    StudentEntity student;
        //    SubjectEntity subject;
        //    var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        //    using (var context = new SchoolContext(options))
        //    {
        //        // Arrange
        //        student = StudentEntityHelper.CreateRandomStudent();
        //        subject = SubjectEntityHelper.CreateRandomSubject();

        //        context.Students.Add(student);
        //        student.Subjects.Add(subject);
        //        context.SaveChanges();

        //        //Act
        //        student.Subjects.Remove(subject);
        //        context.SaveChanges();
        //    }

        //    // Assert
        //    using (var context = new SchoolContext(options))
        //    {
        //        var actualStudent = await context.Students.Include(i => i.Subjects).SingleAsync(i => i.Id == student.Id);
        //        var actualSubject = await context.Subjects.Include(i => i.Students).SingleAsync(i => i.Id == subject.Id); ;
        //        Assert.False(actualStudent.Subjects.Contains(actualSubject));
        //        Assert.False(actualSubject.Students.Contains(actualStudent));
        //    }
        //}

        //[Fact]
        //public async Task GetAll_Activities_OfAStudent()
        //{
        //    var activitiesOfAStudent = new List<ActivityEntity>();
        //    StudentEntity student;
        //    SubjectEntity subject1;
        //    SubjectEntity subject2;
        //    ActivityEntity activity1;
        //    ActivityEntity activity2;
        //    ActivityEntity activity3;

        //    var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        //    using (var context = new SchoolContext(options))
        //    {
        //        // Arrange
        //        student = StudentEntityHelper.CreateRandomStudent();
        //        subject1 = SubjectEntityHelper.CreateRandomSubject();
        //        subject2 = SubjectEntityHelper.CreateRandomSubject();
        //        context.Students.Add(student);
        //        student.Subjects.Add(subject1);
        //        student.Subjects.Add(subject2);

        //        activity1 = ActivityEntityHelper.CreateRandomActivity(subject1);
        //        activity2 = ActivityEntityHelper.CreateRandomActivity(subject1);
        //        activity3 = ActivityEntityHelper.CreateRandomActivity(subject2);
        //        subject1.Activity.Add(activity1);
        //        subject1.Activity.Add(activity2);
        //        subject2.Activity.Add(activity3);
        //        context.SaveChanges();

        //        // Act
        //        activitiesOfAStudent = student.Subjects.SelectMany(i => i.Activity).ToList();
        //    }

        //    using (var context = new SchoolContext(options))
        //    {
        //        // Assert
        //        var studentFromDb = await context.Students.Include(i => i.Subjects).ThenInclude(i => i.Activity).SingleAsync(i => i.Id == student.Id);
        //        Assert.Equal(student.Id, studentFromDb.Id);
        //        Assert.Equal(3, activitiesOfAStudent.Count);

        //    }
        //}

        //[Fact]
        //public async Task GetAll_Subjects_OfAStudent()
        //{
        //    StudentEntity student;
        //    SubjectEntity subject1;
        //    SubjectEntity subject2;
        //    SubjectEntity subject3;
        //    var subjectsOfStudent = new List<SubjectEntity>();

        //    var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        //    using (var context = new SchoolContext(options))
        //    {
        //        // Arrange
        //        student = StudentEntityHelper.CreateRandomStudent();
        //        subject1 = SubjectEntityHelper.CreateRandomSubject();
        //        subject2 = SubjectEntityHelper.CreateRandomSubject();
        //        subject3 = SubjectEntityHelper.CreateRandomSubject();

        //        context.Students.Add(student);
        //        student.Subjects.Add(subject1);
        //        student.Subjects.Add(subject2);
        //        context.Subjects.Add(subject3);
        //        context.SaveChanges();

        //        // Act
        //        subjectsOfStudent = student.Subjects.ToList();
        //    }

        //    using (var context = new SchoolContext(options))
        //    {
        //        // Assert
        //        var studentFromDb = await context.Students.Include(i => i.Subjects).SingleAsync(i => i.Id == student.Id);
        //        Assert.Equal(student.Id, studentFromDb.Id);
        //        Assert.Equal(2, subjectsOfStudent.Count);

        //        foreach (var subject in subjectsOfStudent)
        //        {
        //            Assert.True(subject.Students.Contains(student));
        //        }
        //    }
                
        //}
    }
}