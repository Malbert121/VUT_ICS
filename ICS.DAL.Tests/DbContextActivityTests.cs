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
                entity = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Subjects.Add(subject);
                subject.activity.Add(activity);

                // Act
                activity.ratings.Add(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.Include(i => i.ratings).SingleAsync(i => i.Id == activity.Id);
                var actualRating = await context.Rating.SingleAsync(i => i.Id == entity.Id);
                Assert.Equal(entity.Id, actualRating.Id);
                Assert.NotNull(actualRating);
            }
        }
        //Test for deleting rating from activity
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
                entity = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Subjects.Add(subject);
                subject.activity.Add(activity);
                activity.ratings.Add(entity);
                context.SaveChanges();

                // Act
                activity.ratings.Remove(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualActivity = await context.Activities.Include(i => i.ratings).SingleAsync(i => i.Id == activity.Id);
                var actualRating = await context.Rating.SingleOrDefaultAsync(i => i.Id == entity.Id);
                Assert.Null(actualRating);
                Assert.False(actualActivity.ratings.Contains(actualRating));
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
                entity = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Students.Add(student);
                student.subjects.Add(subject);
                subject.activity.Add(activity);
                activity.ratings.Add(entity);
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
            var retingsOfActivity = new List<RatingEntity>();
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student = StudentEntityHelper.CreateRandomStudent();
                first_entity = RatingEntityHelper.CreateRandomRating(activity, student);
                second_entity = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Students.Add(student);
                student.subjects.Add(subject);
                subject.activity.Add(activity);
                activity.ratings.Add(first_entity);
                activity.ratings.Add(second_entity);
                context.SaveChanges();

                // Act
                retingsOfActivity = subject.activity.SelectMany(i => i.ratings).ToList();
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
            var studentsOfActivity = new List<StudentEntity>();
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                activity = ActivityEntityHelper.CreateRandomActivity(subject);
                student1 = StudentEntityHelper.CreateRandomStudent();
                student2 = StudentEntityHelper.CreateRandomStudent();
                rating1 = RatingEntityHelper.CreateRandomRating(activity, student1);
                rating2 = RatingEntityHelper.CreateRandomRating(activity, student2);
                context.Subjects.Add(subject);
                subject.students.Add(student1);
                subject.students.Add(student2);
                subject.activity.Add(activity);
                activity.ratings.Add(rating1);
                activity.ratings.Add(rating2);
                context.SaveChanges();

                // Act
                studentsOfActivity = subject.activity.SelectMany(i => i.ratings).Select(i => i.student).ToList();
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
            var ratingsOfStudent = new List<RatingEntity>();
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                // Arrange
                subject = SubjectEntityHelper.CreateRandomSubject();
                student = StudentEntityHelper.CreateRandomStudent();
                activity1 = ActivityEntityHelper.CreateRandomActivity(subject);
                activity2 = ActivityEntityHelper.CreateRandomActivity(subject);
                rating1 = RatingEntityHelper.CreateRandomRating(activity1, student);
                rating2 = RatingEntityHelper.CreateRandomRating(activity2, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity1);
                subject.activity.Add(activity2);
                activity1.ratings.Add(rating1);
                activity2.ratings.Add(rating2);
                context.SaveChanges();

                // Act
                ratingsOfStudent = student.subjects.SelectMany(i => i.activity).SelectMany(i => i.ratings).ToList();
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
                rating1 = RatingEntityHelper.CreateRandomRating(activity, student);
                rating2 = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity);
                activity.ratings.Add(rating1);
                activity.ratings.Add(rating2);
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
                rating1 = RatingEntityHelper.CreateRandomRating(activity, student);
                rating2 = RatingEntityHelper.CreateRandomRating(activity, student);
                context.Subjects.Add(subject);
                subject.students.Add(student);
                subject.activity.Add(activity);
                activity.ratings.Add(rating1);
                activity.ratings.Add(rating2);
                context.SaveChanges();

                // Act
                context.Students.Remove(student);
                context.SaveChanges();
            }

            // Assert
            using (var context = new SchoolContext(new DbContextOptions<SchoolContext>()))
            {
                var actualStudent = await context.Students.SingleOrDefaultAsync(i => i.Id == student.Id);
                var ratingcount = await context.Rating.CountAsync();
                Assert.Equal(0, ratingcount);
                Assert.Null(actualStudent);
            }
        }
         
    }
}

