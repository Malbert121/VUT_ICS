using ICS.Services;
using ICS.Messages;
using ICS.ViewModel.Rating;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;


namespace ICS.ViewModel.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<ActivityEditMessage>
{
    public Guid Id { get; set; }
    public ActivityDetailModel? Activity { get; private set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activity = await activityFacade.GetAsync(Id);

        if (Activity is null)
        {
            await alertService.DisplayAsync("Error", "Subject was not found or deleted");
            MessengerService.Send(new StudentDeleteMessage());
            navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Activity is not null)
        {
            try
            {
                await activityFacade.DeleteAsync(Activity.Id);
                MessengerService.Send(new ActivityDeleteMessage());
                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("Error", "Activity already is deleted");
            }
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(ActivityEditViewModel.Activity)] = Activity! });
    }

    [RelayCommand]
    private async Task GoToRatingAsync()
    {
        await navigationService.GoToAsync("/ratings",
        new Dictionary<string, object?> { [nameof(RatingListViewModel.Ratings)] = Activity!.ratings, [nameof(RatingListViewModel.Activity)] = Activity });
    }

    public async void Receive(ActivityEditMessage message)
    {
        if (message.ActivityId == Activity?.Id)
        {
            await LoadDataAsync();
        }
    }
}

