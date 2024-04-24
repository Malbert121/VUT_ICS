
namespace ICS.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public ICollection<ActivityEntity> Activity { get; init; } = new List<ActivityEntity>();
        public ICollection<StudentEntity> Students { get; init; } = new List<StudentEntity>();
        public required Guid Id { get; set; }
    }
}
