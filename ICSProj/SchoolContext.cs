namespace ICSProj;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

// Classes definition


public class StudentEntity
{
    public int studentId { get; set; }
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string fotoURL { get; set; } = string.Empty;
    public ICollection<SubjectEntity> subjects { get; set; } = new List<SubjectEntity>();
    //public ICollection<Activity> activities { get; set; } = new List<Activity>();
}

public class ActivityEntity
{ 
    public int activityId { get; set; }
    public string name { get; set; } = string.Empty;
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string room { get; set; } = string.Empty;
    public string activityTypeTag { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public int subjectId { get; set; }
    public SubjectEntity? subject { get; set; }
    public RatingEntity? rating { get; set; } 
    
}


public class SubjectEntity
{
    public int subjectId { get; set; }
    public string name { get; set; } = string.Empty;
    public string abbreviation { get; set; } = string.Empty;
    public ICollection<ActivityEntity> activity { get; set; } = new List<ActivityEntity>();
    public ICollection<StudentEntity> students { get; set; } = new List<StudentEntity>();
}

public class RatingEntity
{
    public int ratingId { get; set; }
    public int body { get; set; }
    public string note { get; set; } = string.Empty;
    public int activityId { get; set; }
    public ActivityEntity? activity { get; set; }
    public int studentId { get; set; }
    public StudentEntity? student { get; set; }
}

// DbContext
public class SchoolContext : DbContext
{
    public DbSet<StudentEntity> Students { get; set; }
    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<SubjectEntity> Subjects { get; set; }
    public DbSet<RatingEntity> Rating { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=SchoolApp.db;");
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
        
        //modelBuilder.Entity<User>().ToTable("Users");

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
            .HasOne(a => a.rating)
            .WithOne(r => r.activity)
            .HasForeignKey<RatingEntity>(r => r.activityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //one sided connection from rating to one student
        modelBuilder.Entity<RatingEntity>()
            .HasOne(r => r.student)
            .WithMany()
            .HasForeignKey(r => r.studentId);

        
    }

}
