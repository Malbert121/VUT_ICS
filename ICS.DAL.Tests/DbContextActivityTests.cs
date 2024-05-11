//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using Xunit;
//using Microsoft.Extensions.Options;
//using System.Data;
//using Xunit.Abstractions;
//using Microsoft.EntityFrameworkCore.Internal;
//using ICS.DAL;
//using ICS.DAL.Context;
//using ICS.DAL.Entities;
//using Microsoft.Data.Sqlite;
//using System.IO;

//namespace DAL_Tests
//{
//    public class Activity_and_Rating_Test
//    {

//        [Fact]
//        public async Task Update_Activity_Persisted()
//        {
//            ActivityEntity entity;
//            SubjectEntity subject;
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                entity = ActivityEntityHelper.CreateRandomActivity(subject);
//                context.Subjects.Add(subject);
//                subject.Activity.Add(entity);
//                context.SaveChanges();

//                // Act
//                entity.Name = "Exam";
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.SingleAsync(i => i.Id == entity.Id);
//                Assert.Equal("Exam", actualActivity.Name);
//            }
//        }

//        [Fact]
//        public async Task AddNew_RatingToActivity_Persisted()
//        {
//            RatingEntity entity;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                entity = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Subjects.Add(subject);
//                subject.Activity.Add(activity);

//                // Act
//                activity.Ratings.Add(entity);
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.Include(i => i.Ratings).SingleAsync(i => i.Id == activity.Id);
//                var actualRating = await context.Rating.SingleAsync(i => i.Id == entity.Id);
//                Assert.Equal(entity.Id, actualRating.Id);
//                Assert.NotNull(actualRating);
//            }
//        }

//        [Fact]
//        public async Task Delete_RatingFromActivity_Persisted()
//        {
//            RatingEntity entity;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                entity = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Subjects.Add(subject);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(entity);
//                context.SaveChanges();

//                // Act
//                activity.Ratings.Remove(entity);
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.Include(i => i.Ratings).SingleAsync(i => i.Id == activity.Id);
//                var actualRating = await context.Rating.SingleOrDefaultAsync(i => i.Id == entity.Id);
//                Assert.Null(actualRating);
//            }
//        }


//        [Fact]
//        public async Task Update_Rating_Persisted()
//        {
//            RatingEntity entity;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                entity = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Students.Add(student);
//                student.Subjects.Add(subject);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(entity);
//                context.SaveChanges();

//                // Act
//                entity.Points = 200;
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualRating = await context.Rating.SingleAsync(i => i.Id == entity.Id);
//                Assert.Equal(200, actualRating.Points);
//            }
//        }
        
//        [Fact]
//        public async Task GetAll_Ratings_OfActivity()
//        {
//            RatingEntity first_entity, second_entity;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;
//            var retingsOfActivity = new List<RatingEntity>();
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                first_entity = RatingEntityHelper.CreateRandomRating(activity, student);
//                second_entity = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Students.Add(student);
//                student.Subjects.Add(subject);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(first_entity);
//                activity.Ratings.Add(second_entity);
//                context.SaveChanges();

//                // Act
//                retingsOfActivity = subject.Activity.SelectMany(i => i.Ratings).ToList();
//            }


//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.Include(i => i.Ratings).SingleAsync(i => i.Id == activity.Id);
//                Assert.NotNull(actualActivity);
//                Assert.Equal(2, retingsOfActivity.Count);
//            }
//        }

//        [Fact]
//        public async Task GetAll_Students_OfActivity()
//        {
//            StudentEntity student1, student2;
//            RatingEntity rating1, rating2;
//            ActivityEntity activity;
//            SubjectEntity subject;
//            var studentsOfActivity = new List<StudentEntity?>();
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student1 = StudentEntityHelper.CreateRandomStudent();
//                student2 = StudentEntityHelper.CreateRandomStudent();
//                rating1 = RatingEntityHelper.CreateRandomRating(activity, student1);
//                rating2 = RatingEntityHelper.CreateRandomRating(activity, student2);
//                context.Subjects.Add(subject);
//                subject.Students.Add(student1);
//                subject.Students.Add(student2);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(rating1);
//                activity.Ratings.Add(rating2);
//                context.SaveChanges();

//                // Act
//                studentsOfActivity = subject.Activity.SelectMany(i => i.Ratings).Select(i => i.Student).ToList();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.Include(i => i.Ratings).ThenInclude(i => i.Student).SingleAsync(i => i.Id == activity.Id);
//                Assert.NotNull(actualActivity);
//                Assert.Equal(2, studentsOfActivity.Count);
//            }
//        }

//        [Fact]
//        public async Task GetAll_Rating_OfStudent()
//        {
//            RatingEntity rating1, rating2;
//            ActivityEntity activity1, activity2;
//            StudentEntity student;
//            SubjectEntity subject;
//            var ratingsOfStudent = new List<RatingEntity>();
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                student = StudentEntityHelper.CreateRandomStudent();
//                activity1 = ActivityEntityHelper.CreateRandomActivity(subject);
//                activity2 = ActivityEntityHelper.CreateRandomActivity(subject);
//                rating1 = RatingEntityHelper.CreateRandomRating(activity1, student);
//                rating2 = RatingEntityHelper.CreateRandomRating(activity2, student);
//                context.Subjects.Add(subject);
//                subject.Students.Add(student);
//                subject.Activity.Add(activity1);
//                subject.Activity.Add(activity2);
//                activity1.Ratings.Add(rating1);
//                activity2.Ratings.Add(rating2);
//                context.SaveChanges();

//                // Act
//                ratingsOfStudent = student.Subjects.SelectMany(i => i.Activity).SelectMany(i => i.Ratings).ToList();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualStudent = await context.Students.Include(i => i.Subjects).ThenInclude(i => i.Activity).ThenInclude(i => i.Ratings).SingleAsync(i => i.Id == student.Id);
//                Assert.NotNull(actualStudent);
//                Assert.Equal(2, ratingsOfStudent.Count);
//            }
//        }

//        [Fact]
//        public async Task Delete_ActivityWithRating_Persisted()
//        {
//            RatingEntity rating1, rating2;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;
//            var options = DbContextOptionsConfigurer.ConfigureInMemoryOptions();
//            using (var context = new SchoolContext(options))
//            {
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                rating1 = RatingEntityHelper.CreateRandomRating(activity, student);
//                rating2 = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Subjects.Add(subject);
//                subject.Students.Add(student);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(rating1);
//                activity.Ratings.Add(rating2);
//                context.SaveChanges();

//                // Act
//                context.Activities.Remove(activity);
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualActivity = await context.Activities.SingleOrDefaultAsync(i => i.Id == activity.Id);
//                var ratingcount = await context.Rating.CountAsync();
//                Assert.Equal(0, ratingcount);
//                Assert.Null(actualActivity);
//            }
//        }

//        [Fact]
//        public async Task Delete_StudentWithRating_Persisted()
//        {
//            RatingEntity rating1, rating2;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;

//            var options = DbContextOptionsConfigurer.ConfigureSqliteOptions();
//            using (var context = new SchoolContext(options))
//            {
//                context.Database.EnsureDeleted();
//                context.Database.EnsureCreated();
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                rating1 = RatingEntityHelper.CreateRandomRating(activity, student);
//                rating2 = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Subjects.Add(subject);
//                subject.Students.Add(student);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(rating1);
//                activity.Ratings.Add(rating2);
//                context.SaveChanges();

//                // Act
//                context.Students.Remove(student);
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualStudent = await context.Students.SingleOrDefaultAsync(i => i.Id == student.Id);
//                var ratingcount = await context.Rating.CountAsync();
//                var activitycount = await context.Activities.CountAsync();
//                Assert.Equal(1,activitycount);
//                Assert.Equal(0, ratingcount);
//                Assert.Null(actualStudent);
//                context.Database.EnsureDeleted();
//            }
                
//        }

//        [Fact]
//        public async Task Delete_SubjectWithActivityAndRating_Persisted()
//        {
//            RatingEntity rating1, rating2;
//            ActivityEntity activity;
//            StudentEntity student;
//            SubjectEntity subject;

//            var options = DbContextOptionsConfigurer.ConfigureSqliteOptions();
//            using (var context = new SchoolContext(options))
//            {

//                context.Database.EnsureDeleted();
//                context.Database.EnsureCreated();
//                // Arrange
//                subject = SubjectEntityHelper.CreateRandomSubject();
//                activity = ActivityEntityHelper.CreateRandomActivity(subject);
//                student = StudentEntityHelper.CreateRandomStudent();
//                rating1 = RatingEntityHelper.CreateRandomRating(activity, student);
//                rating2 = RatingEntityHelper.CreateRandomRating(activity, student);
//                context.Subjects.Add(subject);
//                subject.Students.Add(student);
//                subject.Activity.Add(activity);
//                activity.Ratings.Add(rating1);
//                activity.Ratings.Add(rating2);
//                context.SaveChanges();

//                // Act
//                context.Subjects.Remove(subject);
//                context.SaveChanges();
//            }

//            // Assert
//            using (var context = new SchoolContext(options))
//            {
//                var actualSubject = await context.Subjects.SingleOrDefaultAsync(i => i.Id == subject.Id);
//                var ratingcount = await context.Rating.CountAsync();
//                Assert.Equal(0, ratingcount);
//                Assert.Null(actualSubject);
//                context.Database.EnsureDeleted();
//            }
//        }

//    }
//}

