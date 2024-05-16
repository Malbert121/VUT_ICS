using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;
using ICS.ViewModel.Activity;

namespace ICS.ViewModel.Rating;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(StudentId), nameof(StudentId))]
[QueryProperty(nameof(Activity), nameof(Activity))]
[QueryProperty(nameof(Rating), nameof(Rating))]
public partial class RatingEditViewModel(
    IRatingFacade ratingFacade,
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService)
{
    public RatingDetailModel Rating { get; init; } = RatingDetailModel.Empty;
    public ActivityDetailModel? Activity { get; set; }
    public StudentDetailModel? Student { get; set; }
    public Guid StudentId { get; set; } = Guid.Empty;
    public Guid SubjectId { get; set; } = Guid.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {


        if (StudentId != Guid.Empty)
        {
            Rating.StudentId = StudentId;
            Student = await studentFacade.GetAsync(StudentId);
            await ratingFacade.SaveAsync(Rating with { ActivityId = Activity!.Id, Student = Student!.FirstName + " " + Student.LastName });
            MessengerService.Send(new RatingEditMessage { RatingId = Rating.Id });
            navigationService.SendBackButtonPressed();
        }
        else if (Rating.StudentId != Guid.Empty)
        {
            await ratingFacade.SaveAsync(Rating);
            MessengerService.Send(new RatingEditMessage { RatingId = Rating.Id });
            navigationService.SendBackButtonPressed();
        }
        else
        {
            await alertService.DisplayAsync("Error", "Student isn't selected");
        }

        


    }

    [RelayCommand]
    private async Task GoToStudentsAsync()
    {
        await navigationService.GoToAsync("/students",
        new Dictionary<string, object?> { 
            [nameof(RatingStudentSelectViewModel.SubjectId)] = SubjectId, 
            [nameof(RatingStudentSelectViewModel.Activity)] = Activity, 
            [nameof(RatingStudentSelectViewModel.SubjectId)] = Activity!.SubjectId, 
            [nameof(RatingStudentSelectViewModel.Rating)] = Rating });
    }


}
