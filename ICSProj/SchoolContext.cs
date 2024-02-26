namespace ICSProj;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

// Definice tříd
public class User
{
    public int UserId { get; set; }
    public string Jmeno { get; set; }
    public string Prijmeni { get; set; }
    public string FotografieURL { get; set; }
    public string Role { get; set; }
}

public class Student : User
{
    public ICollection<Predmet> Predmety { get; set; } = new List<Predmet>();
    public ICollection<Aktivita> Aktivity { get; set; } = new List<Aktivita>();
}

public class Teacher : User
{
    public ICollection<Predmet> Predmety { get; set; } = new List<Predmet>();
    public ICollection<Aktivita> Aktivity { get; set; } = new List<Aktivita>();
}

public class Administrator : User
{
    // All other cases
}


public class Aktivita
{
    public int AktivitaId { get; set; }
    public string Nazev { get; set; }
    public DateTime Zacatek { get; set; }
    public DateTime Konec { get; set; }
    public string Mistnost { get; set; }
    public string TypTagAktivity { get; set; }
    public string Popis { get; set; }
    public int PredmetId { get; set; }
    public Predmet Predmet { get; set; }
    public ICollection<Hodnoceni> Hodnoceni { get; set; } = new List<Hodnoceni>();
    public ICollection<Student> Studenti { get; set; } = new List<Student>(); // Добавленный список студентов
}


public class Predmet
{
    public int PredmetId { get; set; }
    public string Nazev { get; set; }
    public string Zkratka { get; set; }
    public ICollection<Aktivita> Aktivity { get; set; } = new List<Aktivita>();
    public ICollection<Student> Studenti { get; set; } = new List<Student>();
}

public class Hodnoceni
{
    public int HodnoceniId { get; set; }
    public int Body { get; set; }
    public string Poznamka { get; set; }
    public int AktivitaId { get; set; }
    public Aktivita Aktivita { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
}

// DbContext
public class SchoolContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Aktivita> Aktivity { get; set; }
    public DbSet<Predmet> Predmety { get; set; }
    public DbSet<Hodnoceni> Hodnoceni { get; set; }

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
            .HasDiscriminator<string>("Role")
            .HasValue<Student>("Student")
            .HasValue<Teacher>("Teacher")
            .HasValue<Administrator>("Administrator");

        // Настройка многие-ко-многим между Predmet и Student
        modelBuilder.Entity<Predmet>()
            .HasMany(p => p.Studenti)
            .WithMany(s => s.Predmety)
            .UsingEntity(j => j.ToTable("StudentPredmet"));

        // Настройка многие-ко-многим между Aktivita и Student
        modelBuilder.Entity<Aktivita>()
            .HasMany(a => a.Studenti)
            .WithMany(s => s.Aktivity)
            .UsingEntity(j => j.ToTable("StudentAktivita"));
    }

}
