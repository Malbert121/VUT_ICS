using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;


using ICS.DAL.Entities;
using ICS.DAL.Seeds;

public static class DbContextOptionsConfigurer
{
    public static DbContextOptions<SchoolContext> ConfigureInMemoryOptions()
    {
        var builder = new DbContextOptionsBuilder<SchoolContext>();
        builder.UseInMemoryDatabase("TestDatabase");
        builder.LogTo(Console.WriteLine);
        return builder.Options;
    }

    public static DbContextOptions<SchoolContext> ConfigureSqliteOptions()
    {
        var builder = new DbContextOptionsBuilder<SchoolContext>();
        builder.UseSqlite(@"Data Source=SchoolApp.db;");
        builder.LogTo(Console.WriteLine);
        return builder.Options;
    }
}


namespace ICS.DAL.Context
{
    public class SchoolContext(DbContextOptions contextOptions, bool seedDemoData = false) :  DbContext(contextOptions)
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<RatingEntity> Rating { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     //optionsBuilder.UseSqlite(@"Data Source=SchoolApp.db;");
        //     //optionsBuilder.UseInMemoryDatabase("TestDatabase");
        //     optionsBuilder.EnableSensitiveDataLogging();
        //
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityEntity>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<RatingEntity>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<StudentEntity>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<SubjectEntity>()
                .HasKey(a => a.Id);


            // Many-to-many between Subject and Student
            modelBuilder.Entity<SubjectEntity>()
                .HasMany(p => p.students)
                .WithMany(s => s.subjects)
                .UsingEntity(j => j.ToTable("StudentSubject"));

            //One to many between subject and activities
            modelBuilder.Entity<SubjectEntity>()
                .HasMany(s => s.activity)
                .WithOne(a => a.subject)
                .HasForeignKey(a => a.subjectId)
                .OnDelete(DeleteBehavior.Cascade);

            //many to one between activity and rating
            modelBuilder.Entity<ActivityEntity>()
                .HasMany(a => a.ratings)
                .WithOne(r => r.activity)
                .HasForeignKey(r => r.activityId)
                .OnDelete(DeleteBehavior.Cascade);

            //one sided connection from rating to one student
            modelBuilder.Entity<RatingEntity>()
                .HasOne(r => r.student)
                .WithMany()
                .HasForeignKey(r => r.studentId)
                .OnDelete(DeleteBehavior.Cascade);

            if (seedDemoData)
            {
                ActivitySeeds.Seed(modelBuilder);
                RatingSeeds.Seed(modelBuilder);
                StudentSeeds.Seed(modelBuilder);
                SubjectSeeds.Seed(modelBuilder);
            }
        }

    }
}
