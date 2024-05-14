using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingDetailModel : ModelBase
{
    public int points { get; set; }
    public string note { get; set; } = string.Empty;
    public string student { get; set; } = string.Empty;
    public Guid studentId { get; set; }
    public Guid activityId { get; set; }
    public static RatingDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        points = 0,
        note = string.Empty,
        student = string.Empty,
        studentId = Guid.Empty,
        activityId = Guid.Empty
    };
}
