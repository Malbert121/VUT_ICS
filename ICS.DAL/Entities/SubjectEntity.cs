
namespace ICS.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public ICollection<ActivityEntity> Activity { get; init; } = new List<ActivityEntity>();
        public ICollection<StudentSubjectEntity> Students { get; init; } = new List<StudentSubjectEntity>();
        public required Guid Id { get; set; }
    }
}
