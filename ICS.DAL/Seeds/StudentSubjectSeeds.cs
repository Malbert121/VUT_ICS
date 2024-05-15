using ICS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.Seeds
{
    public static class StudentSubjectSeeds
    {
        public static readonly StudentSubjectEntity HarryPotions = new()
        {
            StudentId = StudentSeeds.student1.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.student1,
            Subject = SubjectSeeds.potions,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-9140cba37334")
        };

        public static readonly StudentSubjectEntity HermionePotions = new()
        {
            StudentId = StudentSeeds.student2.Id,
            SubjectId = SubjectSeeds.potions.Id,
            Student = StudentSeeds.student2,
            Subject = SubjectSeeds.potions,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-7140cba37334")
        };
        public static readonly StudentSubjectEntity HarryDarkArts = new()
        {
            StudentId = StudentSeeds.student1.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student1,
            Subject = SubjectSeeds.DefenceDarkArts,
            Id = Guid.Parse("6e6e0b29-20e6-4723-99d5-5140cba37334")
        };

        public static readonly StudentSubjectEntity HermioneDarkArts = new()
        {
            StudentId = StudentSeeds.student2.Id,
            SubjectId = SubjectSeeds.DefenceDarkArts.Id,
            Student = StudentSeeds.student2,
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

        public static readonly StudentSubjectEntity NevilHerbology = new()
        {
            StudentId = StudentSeeds.student5.Id,
            SubjectId = SubjectSeeds.Herbology.Id,
            Student = StudentSeeds.student5,
            Subject = SubjectSeeds.Herbology,
            Id = Guid.Parse("1e891d88-d8b9-4fe6-b123-61a9e2d69ef5")
        };

        public static readonly StudentSubjectEntity LunaHerbology = new()
        {
            StudentId = StudentSeeds.student4.Id,
            SubjectId = SubjectSeeds.Herbology.Id,
            Student = StudentSeeds.student4,
            Subject = SubjectSeeds.Herbology,
            Id = Guid.Parse("81b2e5d1-52e2-4582-902e-94f6f96ab183")
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSubjectEntity>().HasData(
                HarryPotions with { Student = null!, Subject = null! },
                HermionePotions with { Student = null!, Subject = null! },
                HarryDarkArts with { Student = null!, Subject = null! },
                HermioneDarkArts with { Student = null!, Subject = null! },
                RonaldDarkArts with { Student = null!, Subject = null! },
                LunaDarkArts with { Student = null!, Subject = null! },
                NevilleDarkArts with { Student = null!, Subject = null! },
                NevilHerbology with { Student = null!, Subject = null! },
                LunaHerbology with { Student = null!, Subject = null! });
        }

    }
}
