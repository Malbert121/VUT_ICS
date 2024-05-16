using ICS.DAL.Entities;
using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public string Name { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Room { get; set; } = string.Empty;
    public string ActivityTypeTag { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid SubjectId { get; set; }
    public ObservableCollection<RatingListModel> Ratings { get; set; } = new ();
    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Start = DateTime.Now,
        End = DateTime.Now,
        Room = string.Empty,
        ActivityTypeTag = string.Empty,
        SubjectId = Guid.Empty,
        Description = string.Empty
    };
}        
