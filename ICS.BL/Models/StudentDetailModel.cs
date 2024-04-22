using ICS.DAL.Entities;
using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record StudentDetailModel : ModelBase
{
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string photoURL { get; set; } = string.Empty;
    public ObservableCollection<SubjectListModel> subjects { get; set; } = new();
    public static StudentDetailModel Empty => new()
    {
        Id = Guid.Empty,
        firstName = string.Empty,
        lastName = string.Empty,
        photoURL = string.Empty,
        subjects = new ObservableCollection<SubjectListModel>()
    };
}
