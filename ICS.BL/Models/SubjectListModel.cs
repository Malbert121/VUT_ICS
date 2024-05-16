namespace ICS.BL.Models;

public record SubjectListModel : ModelBase
{
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public static SubjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Abbreviation = string.Empty
    };
}
