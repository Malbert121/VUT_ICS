

namespace ICS.DAL.Entities
{
    public record StudentEntity : IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public ICollection<SubjectEntity> Subjects { get; set; } = new List<SubjectEntity>();

        public required Guid Id { get; set; }

    }
}
