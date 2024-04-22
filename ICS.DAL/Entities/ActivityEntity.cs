

namespace ICS.DAL.Entities
{
    public record ActivityEntity : IEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Room { get; set; } = string.Empty;
        public string ActivityTypeTag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public SubjectEntity? Subject { get; set; }
        public ICollection<RatingEntity> Ratings { get; init; } = new List<RatingEntity>();

        public required Guid Id { get; set; }

    }
}
