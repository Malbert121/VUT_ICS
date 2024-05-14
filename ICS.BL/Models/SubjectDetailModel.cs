using ICS.DAL.Entities;
using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record SubjectDetailModel : ModelBase
{
    public string name { get; set; } = string.Empty;
    public string abbreviation { get; set; } = string.Empty;
    public ObservableCollection<ActivityListModel> activity { get; init; } = new();
    public ObservableCollection<StudentSubjectListModel> students { get; init; } = new();
    public static SubjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        name = string.Empty,
        abbreviation = string.Empty,
        activity = new(),
        students = new()
    };
}
