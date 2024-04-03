

namespace ICS.DAL.Entities
{
    public class ActivityEntity : IEntity
    {
        public string name { get; set; } = string.Empty;
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string room { get; set; } = string.Empty;
        public string activityTypeTag { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public Guid subjectId { get; set; }
        public SubjectEntity? subject { get; set; }
        public ICollection<RatingEntity> ratings { get; set; } = new List<RatingEntity>();

        public required Guid Id { get; set; }

    }
}
