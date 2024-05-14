namespace ICS.BL.Models;

public record SubjectListModel : ModelBase
{
    public string name { get; set; } = string.Empty;
    public string abbreviation { get; set; } = string.Empty;
    public static SubjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        name = string.Empty,
        abbreviation = string.Empty
    };
}
