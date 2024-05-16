namespace ICS.BL.Models;

public record StudentListModel : ModelBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public static StudentListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        FirstName = string.Empty,
        LastName = string.Empty
    };
}
