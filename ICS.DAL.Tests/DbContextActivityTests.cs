using ICS.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using System.Data;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Diagnostics;
using ICS.DAL.Context;
using ICS.DAL.Entities;

namespace DAL_Tests
{
    public class Activity_and_Rating_Test
    {

        [Fact]
        public async Task Update_Activity_Persisted()
        {
            ActivityEntity entity;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                entity = ActivityEntityHelper.CreateRandomActivity(subject);
                context.Subjects.Add(subject);
                subject.activity.Add(entity);
                context.SaveChanges();

                // Act
                entity.name = "Exam";
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal("Exam", actualActivity.name);
            }
        }

        [Fact]
        public async Task AddNew_RatingToActivity_Persisted()
        {
            RatingEntity entity;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                entity = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Subjects.Add(subject);
                subject.activity.Add(activity);

                // Act
                activity.rating.Add(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.Include(i => i.rating).SingleAsync(i => i.Id == activity.Id);
                var actualRating = await context.Rating.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal(entity.Id, actualRating.Id);
                Assert.NotNull(actualRating);
            }
        }

        [Fact]
        public async Task Delete_RatingFromActivity_Persisted()
        {
            RatingEntity entity;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                entity = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Subjects.Add(subject);
                subject.activity.Add(activity);
                activity.rating.Add(entity);
                context.SaveChanges();

                // Act
                activity.rating.Remove(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.Include(i => i.rating).SingleAsync(i => i.Id == activity.Id);
                var actualRating = await context.Rating.SingleOrDefaultAsync(i => i.Id == entity.Id);
                Assert.Null(actualRating);
                Assert.Null(actualActivity.rating);
            }
        }

        [Fact]
        public async Task Update_Rating_Persisted()
        {
            RatingEntity entity;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                entity = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Student.Add(student);
                student.subjects.Add(subject);
                subject.activity.Add(activity);
                activity.rating.Add(entity);
                context.SaveChanges();

                // Act
                entity.points = 200;
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualRating = await context.Rating.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal(200, actualRating.points);
            }
        }

        [Fact]
        public async Task GetAll_Ratings_OfActivity()
        {
            RatingEntity first_entity, second_entity;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                first_entity = RatingEntityHelper.CreateRandomRating(subject, student);
                second_entity = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Student.Add(student);
                student.subjects.Add(subject);
                subject.activity.Add(activity);
                activity.rating.Add(first_entity);
                activity.rating.Add(second_entity);
                context.SaveChanges();

                // Act
                var retingsOfActivity = activity.rating.SelectMany(in => in.rating).ToList();
            }


            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                Assert.Equal(2, retingsOfActivity.Count);
            }
        }

        [Fact]
        public async Task GetAll_Students_OfActivity()
        {
            StudentEntity student1, student2;
            RatingEntity rating1, rating2;
            ActivityEntity activity;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student1 = StudentEntityHelper.CreateRandomStudent();
                student2 = StudentEntityHelper.CreateRandomStudent();
                rating1 = RatingEntityHelper.CreateRandomRating(subject, student1);
                rating2 = RatingEntityHelper.CreateRandomRating(subject, student2);
                context.Subjects.Add(subject);
                subject.students.Add(student1);
                subject.students.Add(student2);
                subject.activity.Add(activity);
                activity.rating.Add(rating1);
                activity.rating.Add(rating2);
                context.SaveChanges();

                // Act
                var studentsOfActivity = activity.rating.SelectMany(i => i.student).ToList();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                Assert.Equal(2, studentsOfActivity.Count);
            }
        }

        [Fact]
        public async Task GetAll_Rating_OfStudent()
        {
            RatingEntity rating1, rating2;
            ActivityEntity activity1, activity2;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                student = StudentEntityHelper.CreateRandomStudent();
                activity1 = ActivityEntityHelper.CreateRandomActivity(subject);
                activity2 = ActivityEntityHelper.CreateRandomActivity(subject);
                rating1 = RatingEntityHelper.CreateRandomRating(subject, student);
                rating2 = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity1);
                subject.activity.Add(activity2);
                activity1.rating.Add(rating1);
                activity2.rating.Add(rating2);
                context.SaveChanges();

                // Act
                var ratingsOfStudent = student.subjects.SelectMany(i => i.activity).SelectMany(i => i.rating).ToList();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                Assert.Equal(2, ratingsOfStudent.Count);
            }
        }

        [Fact]
        public async Task Delete_ActivityWithRating_Persisted()
        {
            RatingEntity rating1, rating2;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                rating1 = RatingEntityHelper.CreateRandomRating(subject, student);
                rating2 = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity);
                activity.rating.Add(rating1);
                activity.rating.Add(rating2);
                context.SaveChanges();

                // Act
                context.Activities.Remove(activity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.SingleOrDefaultAsync(i => i.Id == activity.Id);
                var ratingcount = await context.Rating.CountAsync();
                Assert.Equal(0, ratingcount);
                Assert.Null(actualActivity);
            }
        }

        [Fact]
        public async Task Delete_StudentWithRating_Persisted()
        {
            RatingEntity rating1, rating2;
            ActivityEntity activity;
            StudentEntity student;
            SubjectEntity subject;
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                rating1 = RatingEntityHelper.CreateRandomRating(subject, student);
                rating2 = RatingEntityHelper.CreateRandomRating(subject, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity);
                activity.rating.Add(rating1);
                activity.rating.Add(rating2);
                context.SaveChanges();

                // Act
                context.Student.Remove(student);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualStudent = await context.Student.SingleOrDefaultAsync(i => i.Id == student.Id);
                var ratingcount = await context.Rating.CountAsync();
                Assert.Equal(0, ratingcount);
                Assert.Null(actualStudent);
            }
        }
         
    }
}

