using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<RatingEntity> Rating { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
            optionsBuilder.LogTo(Console.WriteLine);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityEntity>()
                .HasKey(a => a.activityId);
            modelBuilder.Entity<RatingEntity>()
                .HasKey(a => a.ratingId);
            modelBuilder.Entity<StudentEntity>()
                .HasKey(a => a.studentId);
            modelBuilder.Entity<SubjectEntity>()
                .HasKey(a => a.subjectId);


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

            //one to one between activity and rating
            modelBuilder.Entity<ActivityEntity>()
                .HasMany(a => a.ratings)
                .WithOne(r => r.activity)
                .HasForeignKey(r => r.activityId)
                .OnDelete(DeleteBehavior.Cascade);

            //one sided connection from rating to one student
            modelBuilder.Entity<RatingEntity>()
                .HasOne(r => r.student)
                .WithMany()
                .HasForeignKey(r => r.studentId);


        }

    }
}
