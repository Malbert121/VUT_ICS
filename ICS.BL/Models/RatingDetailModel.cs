using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingDetailModel : ModelBase
{
    public int points { get; set; }
    public string note { get; set; } = string.Empty;
    public string Student { get; set; } = string.Empty;
    public Guid studentId { get; set; }
    public Guid activityId { get; set; }
    public static RatingDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        points = 0,
        note = string.Empty,
        Student = string.Empty,
        studentId = Guid.Empty,
        activityId = Guid.Empty
    };
}
