using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Activity;

[QueryProperty(nameof(Activity), nameof(Activity))]
[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public SubjectDetailModel Subject { get; init; }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Subject is not null)
        {
            await activityFacade.SaveAsync(Activity with { subjectId = Subject.Id, ratings = default! });
        }
        else
        {
            await activityFacade.SaveAsync(Activity with { ratings = default! });
        }
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });

        navigationService.SendBackButtonPressed();
    }
}
