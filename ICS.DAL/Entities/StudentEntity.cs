

namespace ICS.DAL.Entities
{
    public class StudentEntity : IEntity
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string fotoURL { get; set; } = string.Empty;
        public ICollection<SubjectEntity> subjects { get; set; } = new List<SubjectEntity>();

        public required Guid Id { get; set; }

    }
}
