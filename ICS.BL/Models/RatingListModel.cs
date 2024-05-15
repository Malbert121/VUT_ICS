using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingListModel : ModelBase
{
    public int points { get; set; }
    public Guid activityId { get; set; }
    public Guid studentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public static RatingListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        points = 0,
        studentId = Guid.Empty,
        StudentName = string.Empty,
        activityId = Guid.Empty,
    };
}
