namespace ICS.BL.Models;

public record ActivityListModel : ModelBase
{
    public string name { get; set; } = string.Empty;
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string room { get; set; } = string.Empty;
    public Guid subjectId { get; set; }

    public static ActivityListModel Empty => new()
    {
        Id = Guid.Empty,
        name = string.Empty,
        start = DateTime.MinValue,
        end = DateTime.MinValue,
        room = string.Empty,
        subjectId = Guid.Empty
    };
}
