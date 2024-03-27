

namespace ICS.DAL.Entities
{
    public class RatingEntity : IEntity
    {
        public int points { get; set; }
        public string note { get; set; } = string.Empty;
        public Guid activityId { get; set; }
        public ActivityEntity? activity { get; set; }
        public Guid studentId { get; set; }
        public required StudentEntity? student { get; set; }

        public required Guid Id { get; set; }
    }
}
