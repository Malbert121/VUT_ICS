using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.Common.Tests.Seeds
{
    public static class StudentSubjectSeeds
    {
        public static readonly StudentSubjectEntity HarryPotions = new()
        {
            StudentId = StudentSeeds.Harry.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.Harry,
            Subject = SubjectSeeds.potions,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140cba37334")
        };

        public static readonly StudentSubjectEntity HermionePotions = new()
        {
            StudentId = StudentSeeds.Hermione.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.Hermione,
            Subject = SubjectSeeds.potions,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-7140cba37334")
        };
        public static readonly StudentSubjectEntity HarryDarkArts = new()
        {
            StudentId = StudentSeeds.Harry.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.Harry,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-5140cba37334")
        };

        public static readonly StudentSubjectEntity HermioneDarkArts = new()
        {
            StudentId = StudentSeeds.Hermione.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.Hermione,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-4140cba37334")
        };
        public static readonly StudentSubjectEntity RonaldDarkArts = new()
        {
            StudentId = StudentSeeds.student3.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student3,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-3140cba37334")
        };

        public static readonly StudentSubjectEntity LunaDarkArts = new()
        {
            StudentId = StudentSeeds.student4.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student4,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-2140cba37334")
        };

        public static readonly StudentSubjectEntity NevilleDarkArts = new()
        {
            StudentId = StudentSeeds.student5.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student5,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-1140cba37334")
        };

        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<StudentSubjectEntity>().HasData(
                HarryPotions with { Student = null!, Subject = null! },
                HermionePotions with { Student = null!, Subject = null! },
                HarryDarkArts with { Student = null!, Subject = null! },
                HermioneDarkArts with { Student = null!, Subject = null! },
                RonaldDarkArts with { Student = null!, Subject = null! },
                LunaDarkArts with { Student = null!, Subject = null! },
                NevilleDarkArts with { Student = null!, Subject = null! }
            );
    }
}

