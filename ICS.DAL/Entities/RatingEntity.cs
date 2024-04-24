

namespace ICS.DAL.Entities
{
    public record RatingEntity : IEntity
    {
        public int Points { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid ActivityId { get; set; }
        public ActivityEntity? Activity { get; init; }
        public Guid StudentId { get; set; }
        public StudentEntity? Student { get; init; }

        public required Guid Id { get; set; }
    }
}
