namespace ICSProj;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

// Classes definition
public class User
{
    public int userId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string fotoURL { get; set; }
    public string role { get; set; }
}

public class Student : User
{
    public ICollection<Subject> subjects { get; set; } = new List<Subject>();
    public ICollection<Activity> activities { get; set; } = new List<Activity>();
}

public class Teacher : User
{
    public ICollection<Subject> subjects { get; set; } = new List<Subject>();
    public ICollection<Activity> activities { get; set; } = new List<Activity>();
}

public class Administrator : User
{
    // All other cases
}


public class Activity
{
    public int activityId { get; set; }
    public string name { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string room { get; set; }
    public string activityTypeTag { get; set; }
    public string description { get; set; }
    public int subjectId { get; set; }
    public Subject subject { get; set; }
    public ICollection<Rating> rating { get; set; } = new List<Rating>();
    public ICollection<Student> students { get; set; } = new List<Student>(); // added student List
}


public class Subject
{
    public int subjectId { get; set; }
    public string name { get; set; }
    public string abbreviation { get; set; }
    public ICollection<Activity> aktivity { get; set; } = new List<Activity>();
    public ICollection<Student> students { get; set; } = new List<Student>();
}

public class Rating
{
    public int ratingId { get; set; }
    public int body { get; set; }
    public string note { get; set; }
    public int activityId { get; set; }
    public Activity Activity { get; set; }
    public int studentId { get; set; }
    public Student student { get; set; }
}

// DbContext
public class SchoolContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Rating> Rating { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=SchoolApp.db;");
        optionsBuilder.LogTo(Console.WriteLine);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("role")
            .HasValue<Student>("Student")
            .HasValue<Teacher>("Teacher")
            .HasValue<Administrator>("Administrator");

        // Many-to-many between Subject and Student
        modelBuilder.Entity<Subject>()
            .HasMany(p => p.students)
            .WithMany(s => s.subjects)
            .UsingEntity(j => j.ToTable("StudentSubject"));

        // Many-to-many between Activity and Student
        modelBuilder.Entity<Activity>()
            .HasMany(a => a.students)
            .WithMany(s => s.activities)
            .UsingEntity(j => j.ToTable("StudentActivity"));
    }

}
