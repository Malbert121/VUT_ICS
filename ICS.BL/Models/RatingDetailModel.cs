using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingDetailModel : ModelBase
{
    public int Points { get; set; }
    public string Note { get; set; } = string.Empty;
    public string Student { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public Guid ActivityId { get; set; }
    public static RatingDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Points = 0,
        Note = string.Empty,
        Student = string.Empty,
        StudentId = Guid.Empty,
        ActivityId = Guid.Empty
    };
}
