using System;
using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests2.Seeds
{
    public static class SubjectSeeds
    {
        public static SubjectEntity EmptySubject = new SubjectEntity
        {
            Id = Guid.Empty,
            name = string.Empty,
            abbreviation = string.Empty,
        };

        public static SubjectEntity potions = new SubjectEntity
        {
            Id = Guid.Parse("23b3902d-7d4f-4213-9cf0-112348f56238"),
            name = "Potions",
            abbreviation = "POT",

        };

        public static readonly SubjectEntity SubjestWithNoStudent = potions with { Id = Guid.Parse("6E6E0B29-20D6-4723-99D5-9140CBAB73F5"), subjects = Array.Empty<StudentEntity> }; //todo change to SubjectStudentEntity??
        public static readonly SubjectEntity SubjestWithOneStudent = potions with { Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"), subjects = new List<StudentEntity> };
        public static readonly SubjectEntity SubjestWithTwoStudents = potions with { Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"), subjects = new List<StudentEntity> };
        public static readonly SubjectEntity SubjectUpdate = potions with { Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052"), subjects = Array.Empty<StudentEntity> };
        public static readonly SubjectEntity SubjectDelete = potions with { Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619"), subjects = Array.Empty<StudentEntity> };

        static SubjectSeeds()
        {
            SubjestWithOneStudent.students.Add(StudentSeeds.Harry);
            SubjestWithTwoStudents.students.Add(StudentSeeds.Harry);
            SubjestWithTwoStudents.students.Add(StudentSeeds.Hermione);
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectEntity>().HasData(potions, SubjestWithNoStudent, SubjestWithOneStudent, SubjestWithTwoStudents, SubjectUpdate, SubjectDelete);
        }

    }
}




