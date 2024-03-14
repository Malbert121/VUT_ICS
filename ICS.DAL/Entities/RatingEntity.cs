

namespace ICS.DAL.Entities
{
    public class RatingEntity : IEntity
    {
        public int ratingId { get; set; }
        public int points { get; set; }
        public string note { get; set; } = string.Empty;
        public int activityId { get; set; }
        public ActivityEntity? activity { get; set; }
        public int studentId { get; set; }
        public StudentEntity? student { get; set; }

        public required Guid Id { get; set; }
    }
}
