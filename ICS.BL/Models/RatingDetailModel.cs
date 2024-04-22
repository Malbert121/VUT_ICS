using ICS.DAL.Entities;

namespace ICS.BL.Models;

public record RatingDetailModel : ModelBase
{
    public int points { get; set; }
    public string note { get; set; } = string.Empty;
    public Guid activityId { get; set; }
    public ActivityListModel? activity { get; set; } = new ActivityListModel();
    public Guid studentId { get; set; }
    public StudentListModel? student { get; set; } = new StudentListModel();
    public static RatingDetailModel Empty => new()
    {
        Id = Guid.Empty,
        points = 0,
        note = string.Empty,
        studentId = Guid.Empty,
        activityId = Guid.Empty,
        student = new(),
        activity = new()
    };
}
