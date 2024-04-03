using System;
using ICS.DAL.Entities;
using ICS.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests2.Seeds
{
    public static class StudentSeeds
    {
        public static readonly StudentEntity EmptyEntity = new StudentEntity
        {
            Id = Guid.Empty,
            firstName = string.Empty,
            lastName = string.Empty,
            fotoURL = string.Empty
        };

        public static readonly StudentEntity Harry = new StudentEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            firstName = "Harry",
            lastName = "Potter",
            fotoURL = "http://www.example.com/index.html"
        };

        public static readonly StudentEntity Hermione = new StudentEntity
        {
            Id = Guid.Parse("df935095-8709-4040-a2bb-b6f97cb416dc"),
            firstName = "Hermione",
            lastName = "Granger",
            fotoURL = "http://www.example.com/index.html"
        };

        public static readonly StudentEntity StudentWithNoSubjects = Harry with { Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), subjects = Array.Empty<SubjectEntity>() };//SubjectStudentEntity??
        public static readonly StudentEntity StudentWithSubjects = Harry with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), subjects = new List<SubjectEntity>() };
        public static readonly StudentEntity StudentUpdate = Harry with { Id = Guid.Parse("F3A3E3A3-7B1A-48C1-9796-D2BAC7F67868"), subjects = Array.Empty<SubjectEntity>() };
        public static readonly StudentEntity StudentDelete = Harry with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), subjects = Array.Empty<SubjectEntity>() };

        static StudentSeeds()
        {
            StudentWithSubjects.subjects.Add(SubjectSeeds.SubjectEntity);//todo change to SubjectStudentEntity??
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>().HasData(Harry, Hermione, StudentWithNoSubjects, StudentWithSubjects, StudentUpdate, StudentDelete);
        }


    }
}


