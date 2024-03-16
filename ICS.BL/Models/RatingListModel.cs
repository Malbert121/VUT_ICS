using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingListModel : ModelBase
{
    public int points { get; set; }
    public string note { get; set; } = string.Empty;
    public int activityId { get; set; }
    public int studentId { get; set; }
    public StudentEntity? student { get; set; }
    public static RatingListModel Empty => new()
    {
        Id = Guid.Empty,
        points = 0,
        note = string.Empty,
        studentId = 0,
        student = null
    };
}