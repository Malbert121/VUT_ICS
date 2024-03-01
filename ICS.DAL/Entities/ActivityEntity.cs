

namespace ICS.DAL
{
    public class ActivityEntity : IEntity
    {
        public int activityId { get; set; }
        public string name { get; set; } = string.Empty;
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string room { get; set; } = string.Empty;
        public string activityTypeTag { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int subjectId { get; set; }
        public SubjectEntity? subject { get; set; }
        public RatingEntity? rating { get; set; }

        public required Guid Id { get; set; }

    }
}
