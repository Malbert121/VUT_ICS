
namespace ICS.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public string name { get; set; } = string.Empty;
        public string abbreviation { get; set; } = string.Empty;
        public ICollection<ActivityEntity> activity { get; set; } = new List<ActivityEntity>();
        public ICollection<StudentEntity> students { get; set; } = new List<StudentEntity>();
        public required Guid Id { get; set; }
    }
}
