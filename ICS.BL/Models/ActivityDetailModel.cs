using ICS.DAL.Entities;
using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public string name { get; set; } = string.Empty;
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string room { get; set; } = string.Empty;
    public string activityTypeTag { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public int subjectId { get; set; }
    public SubjectEntity? subject { get; set; }
    public RatingEntity? rating { get; set; }
    public ObservableCollection<StudentListModel> ratings { get; set; } = new ();
    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        name = string.Empty,
        start = DateTime.MinValue,
        end = DateTime.MinValue,
        room = string.Empty,
        activityTypeTag = string.Empty,
        subjectId = 0,
        description = string.Empty
    };
}        