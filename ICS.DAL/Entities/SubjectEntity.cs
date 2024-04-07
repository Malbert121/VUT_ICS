
namespace ICS.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public ICollection<ActivityEntity> Activity { get; set; } = new List<ActivityEntity>();
        public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();
        public required Guid Id { get; set; }
    }
}
