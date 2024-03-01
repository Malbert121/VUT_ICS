using ICS.DAL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ICS.DAL.Tests;

public class DbContextSubjectTests
{
    [Fact]
    public async Task AddNew_Subject_Persisted()
    {
        SubjectEntity entity;
        using (var context = new SchoolContext())
        {
            // Arrange
            entity = new SubjectEntity
            {
                Id = Guid.Empty,
                name = "Database Systems",
                abbreviation = "IDS"
            };

            // Act
            context.Subjects.Add(entity);
            context.SaveChanges();
        }

        // Assert
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.SingleAsync(i => i.subjectId == entity.subjectId);
            Assert.NotNull(subject);
            Assert.Equal(entity.subjectId, subject.subjectId);
        }
    }

    [Fact]
    public async Task Delete_Subject_Persisted()
    {
        SubjectEntity entity;
        using (var context = new SchoolContext())
        {
            // Arrange
            entity = new SubjectEntity
            {
                Id = Guid.Empty,
                name = "Computer Communications and Networks",
                abbreviation = "IPK"
            };

            context.Subjects.Add(entity);
            context.SaveChanges();
        }

        // Act
        using (var context = new SchoolContext())
        {
            context.Subjects.Remove(await context.Subjects.FindAsync(entity.subjectId));
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new SchoolContext())
        {
            Assert.False(await context.Subjects.AnyAsync(i => i.subjectId == entity.subjectId));
        }
    }

    [Fact]
    public async Task Update_Subject_Persisted()
    {
        SubjectEntity entity;
        using (var context = new SchoolContext())
        {
            // Arrange
            entity = new SubjectEntity
            {
                Id = Guid.Empty,
                name = "Computer Communications and Networks",
                abbreviation = "IPK"
            };

            context.Subjects.Add(entity);
            context.SaveChanges();
        }

        // Act
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.SingleAsync(i => i.subjectId == entity.subjectId);
            if (subject != null)
            {
                subject.name = "Computer Communications";
                await context.SaveChangesAsync();
            }
        }

        // Assert
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.SingleAsync(i => i.subjectId == entity.subjectId);
            Assert.NotNull(subject);
            Assert.Equal(entity.subjectId, subject.subjectId);
            Assert.Equal("Computer Communications", subject.name);
        }
    }

    [Fact]
    public async Task AddNew_StudentToSubject_Persisted()
    {

        int studentId;
        int subjectId;
        // Arrange
        using (var context = new SchoolContext())
        {
            var student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
            var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

            context.Subjects.Add(subject);
            context.SaveChanges();

            //Act
            subject.students.Add(student);
            context.SaveChanges();

            studentId = student.studentId;
            subjectId = subject.subjectId;
        }

        // Assert
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.Include(i => i.students).SingleAsync(i => i.subjectId == subjectId);
            var student = await context.Students.FindAsync(studentId);
            Assert.True(subject.students.Contains(student));
            Assert.True(student.subjects.Contains(subject));
        }
    }

    [Fact]
    public async Task AddNew_ActivityToSubject_Persisted()
    {
        // Arrange
        int activityId;
        int subjectId;
        using (var context = new SchoolContext())
        {
            var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

            context.Subjects.Add(subject);
            context.SaveChanges();
            subjectId = subject.subjectId;

            var activity = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject, subjectId = subjectId, room = "D105", activityTypeTag = "S", description = "Fundamentals seminar" };

            // Act
            subject.activity.Add(activity);
            context.SaveChanges();
            activityId = activity.activityId;

        }

        // Assert
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.Include(i => i.activity).SingleAsync(i => i.subjectId == subjectId);
            var activity = await context.Activities.FindAsync(activityId);
            Assert.True(subject.activity.Contains(activity));
        }
    }

    [Fact]
    public async Task Delete_ActivityFromSubject_Persisted()
    {
        // Arrange
        int activityId;
        int subjectId;
        using (var context = new SchoolContext())
        {
            var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

            context.Subjects.Add(subject);
            context.SaveChanges();
            subjectId = subject.subjectId;

            var activity = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject, subjectId = subjectId, room = "D105", activityTypeTag = "S", description = "Fundamentals seminar" };

            subject.activity.Add(activity);
            context.SaveChanges();
            activityId = activity.activityId;
        }

        // Act
        using (var context = new SchoolContext())
        {
            var activity = await context.Activities.FindAsync(activityId);
            var subject = await context.Subjects.FindAsync(subjectId);

            subject.activity.Remove(activity);
            context.SaveChanges();

        }

        // Assert
        using (var context = new SchoolContext())
        {
            var subject = await context.Subjects.Include(i => i.activity).SingleAsync(i => i.subjectId == subjectId);
            Assert.False(await context.Activities.AnyAsync(i => i.activityId == activityId));
            Assert.False(subject.activity.Any(a => a.activityId == activityId));

        }
    }

    [Fact]
    public async Task GetAll_Students_OfASubject()
    {
        SubjectEntity subject;
        using (var context = new SchoolContext())
        {
            // Arrange
            subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };
            var student1 = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
            var student2 = new StudentEntity { Id = Guid.Empty, firstName = "Damir", lastName = "Matveev", fotoURL = "http://www.example.com/index.html" };
            var student3 = new StudentEntity { Id = Guid.Empty, firstName = "Svetlana", lastName = "Eliseeva", fotoURL = "http://www.example.com/index.html" };

            context.Subjects.Add(subject);
            subject.students.Add(student1);
            subject.students.Add(student3);

            context.Students.Add(student2);
            context.SaveChanges();
        }

        using (var context = new SchoolContext())
        {
            // Act
            var subjectToGetStudents = await context.Subjects.Include(i => i.students).FirstOrDefaultAsync(i => i.subjectId == subject.subjectId);
            var studentsOfSubject = subjectToGetStudents.students.ToList();

            // Assert
            Assert.Equal(2, studentsOfSubject.Count);

            foreach (var student in studentsOfSubject)
            {
                Assert.True(student.subjects.Contains(subjectToGetStudents));
            }

        }
    }

    [Fact]
    public async Task GetAll_Activities_OfASubject()
    {
        SubjectEntity subject1;
        using (var context = new SchoolContext())
        {
            // Arrange
            subject1 = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };
            var subject2 = new SubjectEntity { Id = Guid.Empty, name = "Database Systems", abbreviation = "IDS" };
            var activity1 = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject1, subjectId = subject1.subjectId, room = "D0207", activityTypeTag = "S", description = "Fundamentals seminar" };
            var activity2 = new ActivityEntity { Id = Guid.Empty, name = "Lecture", subject = subject1, subjectId = subject1.subjectId, room = "D105", activityTypeTag = "L", description = "3h lecture" };
            var activity3 = new ActivityEntity { Id = Guid.Empty, name = "Seminar", subject = subject2, subjectId = subject2.subjectId, room = "D0206", activityTypeTag = "S", description = "Fundamentals seminar" };

            context.Subjects.Add(subject1);
            context.Subjects.Add(subject2);

            subject1.activity.Add(activity1);
            subject1.activity.Add(activity2);
            subject2.activity.Add(activity3);

            context.SaveChanges();
        }

        using (var context = new SchoolContext())
        {
            // Act
            var subjectToGetActivities = await context.Subjects.Include(i => i.activity).FirstOrDefaultAsync(i => i.subjectId == subject1.subjectId);
            var activitiesOfSubject = subjectToGetActivities.activity.ToList();

            // Assert
            Assert.Equal(2, activitiesOfSubject.Count);

            foreach (var activity in activitiesOfSubject)
            {
                Assert.Equal(activity.subjectId, subjectToGetActivities.subjectId);
            }

        }
    }

    [Fact]
    public async Task Delete_StudentFromSubject_Persisted()
    {
        int studentId;
        int subjectId;
        using (var context = new SchoolContext())
        {
            // Arrange
            var student = new StudentEntity { Id = Guid.Empty, firstName = "Peter", lastName = "Adams", fotoURL = "http://www.example.com/index.html" };
            var subject = new SubjectEntity { Id = Guid.Empty, name = "Computer Communications and Networks", abbreviation = "IPK" };

            context.Subjects.Add(subject);
            subject.students.Add(student);
            context.SaveChanges();

            //Act
            subject.students.Remove(student);
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
}
