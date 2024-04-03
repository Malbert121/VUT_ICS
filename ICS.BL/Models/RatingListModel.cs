using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingListModel : ModelBase
{
    public int points { get; set; }
    public Guid activityId { get; set; }
    public Guid studentId { get; set; }
    public StudentEntity? student { get; set; }
    public static RatingListModel Empty => new()
    {
        Id = Guid.Empty,
        points = 0,
        studentId = Guid.Empty,
        activityId = Guid.Empty,
        student = null
    };
}