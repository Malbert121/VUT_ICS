using CommunityToolkit.Mvvm.Input;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.Messages;
using ICS.Services;
using ICS.ViewModel.Activity;
using ICS.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.ViewModel.Rating;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(StudentId), nameof(StudentId))]
[QueryProperty(nameof(Rating), nameof(Rating))]
[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class RatingStudentSelectViewModel(
    IStudentSubjectFacade studentSubjectFacade,
    IStudentFacade studentFacade,
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public IEnumerable<StudentSubjectListModel> Students { get; set; } = null!;
    public StudentDetailModel? Student { get; set; }
    public ActivityDetailModel Activity { get; set; }
    public RatingDetailModel Rating { get; init; } = RatingDetailModel.Empty;
    public Guid StudentId { get; set; } = Guid.Empty;
    public Guid SubjectId { get; set; } = Guid.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Students = await studentSubjectFacade.GetStudentsAsync(SubjectId);
    }

    [RelayCommand]
    private async Task SelectAsync(Guid id)
    {
        var existingRatings = await ratingFacade.GetFromActivityAsync(Activity.Id);
        if (existingRatings.Any(r => r.studentId == id))
        {
            await App.Current.MainPage.DisplayAlert("Error", "This student already marked for this activity.", "OK");
            return;
        }

        Student = await studentFacade.GetAsync(id);
        await navigationService.GoToAsync("//subjects/detail/activities/detail/ratings/edit",
            new Dictionary<string, object?>
            {
                [nameof(RatingEditViewModel.StudentId)] = id,
                [nameof(RatingEditViewModel.Activity)] = Activity,
                [nameof(RatingEditViewModel.SubjectId)] = Activity.subjectId,
                [nameof(RatingEditViewModel.Rating)] = Rating with { Student = Student.firstName + " " + Student.lastName, studentId = id }
            }
                );
    }

}
