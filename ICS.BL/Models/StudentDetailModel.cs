using System.Collections.ObjectModel;

namespace ICS.BL.Models;

public record StudentDetailModel : ModelBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhotoURL { get; set; } = string.Empty;
    public ObservableCollection<StudentSubjectListModel> Subjects { get; set; } = new();
    public static StudentDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        FirstName = string.Empty,
        LastName = string.Empty,
        PhotoURL = string.Empty,
        Subjects = new ObservableCollection<StudentSubjectListModel>()
    };
}
