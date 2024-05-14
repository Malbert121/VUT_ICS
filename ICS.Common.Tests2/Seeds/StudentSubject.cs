using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests.Seeds
{
    public static class StudentSubjectSeeds
    {
        public static readonly StudentSubjectEntity EmptyStudentSubject = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-24e6-4723-99d5-9140cba37334"),
            StudentId = StudentSeeds.EmptyEntity.Id,
            SubjectId = SubjectSeeds.EmptySubject.Id,
            Student = StudentSeeds.EmptyEntity,
            Subject = SubjectSeeds.EmptySubject,

        };

        public static readonly StudentSubjectEntity HarryDarkArts = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-5140cba37334"),
            StudentId = StudentSeeds.Harry.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.Harry,
            Subject = SubjectSeeds.DefenceDarkArts,

        };

        public static readonly StudentSubjectEntity HermioneDarkArts = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-4140cba37334"),
            StudentId = StudentSeeds.Hermione.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.Hermione,
            Subject = SubjectSeeds.DefenceDarkArts,

        };
        public static readonly StudentSubjectEntity RonaldDarkArts = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-3140cba37334"),
            StudentId = StudentSeeds.student3.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student3,
            Subject = SubjectSeeds.DefenceDarkArts,

        };

        public static readonly StudentSubjectEntity LunaDarkArts = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-2140cba37334"),
            StudentId = StudentSeeds.student4.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student4,
            Subject = SubjectSeeds.DefenceDarkArts,

        };

        public static readonly StudentSubjectEntity NevilleDarkArts = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-1140cba37334"),
            StudentId = StudentSeeds.student5.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student5,
            Subject = SubjectSeeds.DefenceDarkArts,

        };

        public static readonly StudentSubjectEntity HarrySubjectWithTwoStudents = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-0140cba37334"),
            StudentId = StudentSeeds.Harry.Id,
            SubjectId = SubjectSeeds.SubjectWithTwoStudents.Id,
            Student = StudentSeeds.Harry,
            Subject = SubjectSeeds.SubjectWithTwoStudents,

        };
        public static readonly StudentSubjectEntity HarryPotions = new StudentSubjectEntity
        {
            Id = Guid.Parse("d9963767-91a2-4b3f-81f7-dc5d0aaecf7d"),
            StudentId = StudentSeeds.Harry.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.Harry,
            Subject = SubjectSeeds.potions,

        };

        public static readonly StudentSubjectEntity HermionePotions = new StudentSubjectEntity
        {
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-7140cba37334"),
            StudentId = StudentSeeds.Hermione.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.Hermione,
            Subject = SubjectSeeds.potions,

        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSubjectEntity>().HasData(
                HarryPotions with { Student = null!, Subject = null! },
                HermionePotions with { Student = null!, Subject = null! },
                HermioneDarkArts with { Student = null!, Subject = null! },
                RonaldDarkArts with { Student = null!, Subject = null! },
                LunaDarkArts with { Student = null!, Subject = null! },
                NevilleDarkArts with { Student = null!, Subject = null! },
                HarryDarkArts with { Student = null!, Subject = null! },
                HarrySubjectWithTwoStudents with { Student = null!, Subject = null! }
            );
        }
    }
}

