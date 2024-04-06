using ICS.DAL;
using ICS.DAL.Context;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
            entity.name = "Computer Communications";
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var subject = await context.Subjects.SingleAsync(i => i.Id == entity.Id);
            Assert.Equal("Computer Communications", subject.name);
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

            context.Subjects.Add(subject);

            //Act
            subject.students.Add(student);
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.students).SingleAsync(i => i.Id == subject.Id);
            var actualStudent = await context.Students.SingleAsync(i => i.Id == student.Id);
            Assert.True(actualSubject.students.Contains(actualStudent));
            Assert.True(actualStudent.subjects.Contains(actualSubject));
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
            subject.activity.Add(activity);
            context.SaveChanges();

        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.activity).SingleAsync(i => i.Id == subject.Id);
            var actualActivity = await context.Activities.SingleAsync(i => i.Id == activity.Id);
            Assert.True(actualSubject.activity.Contains(actualActivity));
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

            subject.activity.Add(activity);
            context.SaveChanges();

            // Act
            subject.activity.Remove(activity);
            context.SaveChanges();

        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualSubject = await context.Subjects.Include(i => i.activity).SingleAsync(i => i.Id == subject.Id);
            Assert.False(await context.Activities.AnyAsync(i => i.Id == activity.Id));
            Assert.DoesNotContain(subject.activity, a => a.Id == activity.Id);

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

            context.Subjects.Add(subject);
            subject.students.Add(student1);
            subject.students.Add(student3);

            context.Students.Add(student2);
            await context.SaveChangesAsync();

            // Act
            var studentsOfSubject = subject.students.ToList();

            // Assert
            Assert.Equal(2, studentsOfSubject.Count);

            foreach (var student in studentsOfSubject)
            {
                Assert.True(student.subjects.Contains(subject));
            }

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

            subject1.activity.Add(activity1);
            subject1.activity.Add(activity2);
            subject2.activity.Add(activity3);

            await context.SaveChangesAsync();

            // Act
            var activitiesOfSubject = subject1.activity.ToList();

            // Assert
            Assert.Equal(2, activitiesOfSubject.Count);

            foreach (var activity in activitiesOfSubject)
            {
                Assert.Equal(activity.subjectId, subject1.Id);
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

            context.Subjects.Add(subject);
            subject.students.Add(student);
            context.SaveChanges();

            //Act
            subject.students.Remove(student);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new SchoolContext(options))
        {
            var actualStudent = await context.Students.Include(i => i.subjects).SingleAsync(i => i.Id == student.Id);
            var actualSubject = await context.Subjects.Include(i => i.students).SingleAsync(i => i.Id == subject.Id); ;
            Assert.False(student.subjects.Contains(subject));
            Assert.False(subject.students.Contains(student));
        }
    }
}
