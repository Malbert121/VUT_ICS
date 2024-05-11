

namespace ICS.DAL.Entities
{
    public record StudentEntity : IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public ICollection<StudentSubjectEntity> Subjects { get; init; } = new List<StudentSubjectEntity>();
        public required Guid Id { get; set; }

    }
}
