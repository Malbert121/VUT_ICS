using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingListModel : ModelBase
{
    public int Points { get; set; }
    public Guid ActivityId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public static RatingListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Points = 0,
        StudentId = Guid.Empty,
        StudentName = string.Empty,
        ActivityId = Guid.Empty,
    };
}
