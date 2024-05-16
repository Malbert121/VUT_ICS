using ICS.DAL.Entities;
using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record SubjectDetailModel : ModelBase
{
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public ObservableCollection<ActivityListModel> Activity { get; init; } = new();
    public ObservableCollection<StudentSubjectListModel> Students { get; init; } = new();
    public static SubjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Abbreviation = string.Empty,
        Activity = new(),
        Students = new()
    };
}
