namespace ICS.BL.Models;

public record StudentListModel : ModelBase
{
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    
    public static StudentListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        firstName = string.Empty,
        lastName = string.Empty
    };
}
