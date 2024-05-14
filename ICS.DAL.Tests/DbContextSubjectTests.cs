using ICS.DAL;
using ICS.DAL.Context;
using ICS.DAL.Entities;
using ICS.DAL.Tests.TestEntityHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace ICS.DAL.Tests;

public class DbContextSubjectTests
{
    [Fact]
    public async Task AddNew_Subject_Persisted()
    {
        SubjectEntity entity;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            entity = SubjectEntityHelper.CreateRandomSubject();

            // Act
            context.Subjects.Add(entity);
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var subject = await context.Subjects.SingleAsync(i => i.Id == entity.Id);
            Assert.NotNull(subject);
            Assert.Equal(entity.Id, subject.Id);
        }
    }

    [Fact]
    public async Task Delete_Subject_Persisted()
    {
        SubjectEntity entity;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            entity = SubjectEntityHelper.CreateRandomSubject();
            context.Subjects.Add(entity);

            // Act
            context.Subjects.Remove(entity);
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            Assert.False(await context.Subjects.AnyAsync(i => i.Id == entity.Id));
        }
    }

    [Fact]
    public async Task Update_Subject_Persisted()
    {
        SubjectEntity entity;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            entity = SubjectEntityHelper.CreateRandomSubject();
            context.Subjects.Add(entity);

            // Act
            entity.Name = "Computer Communications";
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var subject = await context.Subjects.SingleAsync(i => i.Id == entity.Id);
            Assert.Equal("Computer Communications", subject.Name);
        }
    }

    [Fact]
    public async Task AddNew_StudentToSubject_Persisted()
    {
        StudentEntity student;
        SubjectEntity subject;
        // Arrange
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            student = StudentEntityHelper.CreateRandomStudent();
            subject = SubjectEntityHelper.CreateRandomSubject();
            var studentSubject = StudentSubjectEntityHelper.CreateStudentSubject(student, subject);
            context.Subjects.Add(subject);

            //Act
            context.StudentSubjects.Add(studentSubject);
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.Students).SingleAsync(i => i.Id == subject.Id);
            var actualStudent = await context.Students.SingleAsync(i => i.Id == student.Id);
            var studentSubjectInDb = await context.StudentSubjects
                .Include(ss => ss.Subject)
                .Include(ss => ss.Student)
                .FirstOrDefaultAsync(ss => ss.SubjectId == actualSubject.Id && ss.StudentId == actualStudent.Id);

            Assert.NotNull(studentSubjectInDb);
        }
    }

    [Fact]
    public async Task AddNew_ActivityToSubject_Persisted()
    {
        // Arrange
        ActivityEntity activity;
        SubjectEntity subject;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            subject = SubjectEntityHelper.CreateRandomSubject();
            context.Subjects.Add(subject);

            activity = ActivityEntityHelper.CreateRandomActivity(subject);

            // Act
            subject.Activity.Add(activity);
            context.SaveChanges();

        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.Activity).SingleAsync(i => i.Id == subject.Id);
            var actualActivity = await context.Activities.SingleAsync(i => i.Id == activity.Id);
            Assert.True(actualSubject.Activity.Contains(actualActivity));
        }
    }

    [Fact]
    public async Task Delete_ActivityFromSubject_Persisted()
    {
        // Arrange
        ActivityEntity activity;
        SubjectEntity subject;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            subject = SubjectEntityHelper.CreateRandomSubject();
            context.Subjects.Add(subject);

            activity = ActivityEntityHelper.CreateRandomActivity(subject);

            subject.Activity.Add(activity);
            context.SaveChanges();

            // Act
            subject.Activity.Remove(activity);
            context.SaveChanges();

        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.Activity).SingleAsync(i => i.Id == subject.Id);
            Assert.False(await context.Activities.AnyAsync(i => i.Id == activity.Id));
            Assert.DoesNotContain(subject.Activity, a => a.Id == activity.Id);

        }
    }

    [Fact]
    public async Task GetAll_Students_OfASubject()
    {
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            var subject = SubjectEntityHelper.CreateRandomSubject();
            var student1 = StudentEntityHelper.CreateRandomStudent();
            var student2 = StudentEntityHelper.CreateRandomStudent();
            var student3 = StudentEntityHelper.CreateRandomStudent();
            var studentSubject1 = StudentSubjectEntityHelper.CreateStudentSubject(student1, subject);
            var studentSubject2 = StudentSubjectEntityHelper.CreateStudentSubject(student2, subject);
            var studentSubject3 = StudentSubjectEntityHelper.CreateStudentSubject(student3, subject);

            context.Subjects.Add(subject);
            context.Students.Add(student1);
            context.Students.Add(student2);
            context.Students.Add(student3);

            context.StudentSubjects.Add(studentSubject1);
            context.StudentSubjects.Add(studentSubject2);
            context.StudentSubjects.Add(studentSubject3);
            await context.SaveChangesAsync();

            // Act
            var studentsOfSubject = subject.Students.ToList();

            // Assert
            Assert.Equal(3, studentsOfSubject.Count);
            Assert.Contains(student1, studentsOfSubject.Select(ss => ss.Student));
        }
    }

    [Fact]
    public async Task GetAll_Activities_OfASubject()
    {
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            var subject1 = SubjectEntityHelper.CreateRandomSubject();
            var subject2 = SubjectEntityHelper.CreateRandomSubject();
            var activity1 = ActivityEntityHelper.CreateRandomActivity(subject1);
            var activity2 = ActivityEntityHelper.CreateRandomActivity(subject1);
            var activity3 = ActivityEntityHelper.CreateRandomActivity(subject2);

            context.Subjects.Add(subject1);
            context.Subjects.Add(subject2);

            subject1.Activity.Add(activity1);
            subject1.Activity.Add(activity2);
            subject2.Activity.Add(activity3);

            await context.SaveChangesAsync();

            // Act
            var activitiesOfSubject = subject1.Activity.ToList();

            // Assert
            Assert.Equal(2, activitiesOfSubject.Count);

            foreach (var activity in activitiesOfSubject)
            {
                Assert.Equal(activity.SubjectId, subject1.Id);
            }

        }
    }

    [Fact]
    public async Task Delete_StudentFromSubject_Persisted()
    {
        StudentEntity student;
        SubjectEntity subject;
        var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
        using (var context = new SchoolContext(options))
        {
            // Arrange
            student = StudentEntityHelper.CreateRandomStudent();
            subject = SubjectEntityHelper.CreateRandomSubject();
            var studentSubject = StudentSubjectEntityHelper.CreateStudentSubject(student, subject);

            context.Subjects.Add(subject);

            context.StudentSubjects.Add(studentSubject);
            context.SaveChanges();

            //Act
            context.StudentSubjects.Remove(studentSubject);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualStudent = await context.Students.Include(i => i.Subjects).SingleAsync(i => i.Id == student.Id);
            var actualSubject = await context.Subjects.Include(i => i.Students).SingleAsync(i => i.Id == subject.Id); ;
            var studentSubjectInDb = await context.StudentSubjects
                .Include(ss => ss.Subject)
                .Include(ss => ss.Student)
                .FirstOrDefaultAsync(ss => ss.SubjectId == actualSubject.Id && ss.StudentId == actualStudent.Id);

            Assert.Null(studentSubjectInDb);
        }
    }
}
