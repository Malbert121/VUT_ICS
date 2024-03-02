

namespace ICS.DAL
{
    public class StudentEntity : IEntity
    {
        public required Guid Id { get; set; }
        public int studentId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string fotoURL { get; set; } = string.Empty;
        public ICollection<SubjectEntity> subjects { get; set; } = new List<SubjectEntity>();

    }
}
