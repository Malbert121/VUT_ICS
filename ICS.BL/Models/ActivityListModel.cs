namespace ICS.BL.Models;

public record ActivityListModel : ModelBase
{
    public string Name { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Room { get; set; } = string.Empty;
    public Guid SubjectId { get; set; }

    public static ActivityListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Start = DateTime.Now,
        End = DateTime.Now,
        Room = string.Empty,
        SubjectId = Guid.Empty
    };
}
