using ICS.Services;
using ICS.Messages;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ICS.ViewModel.Rating;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(StudentId), nameof(StudentId))]
[QueryProperty(nameof(Rating), nameof(Rating))]
[QueryProperty(nameof(Activity), nameof(Activity))]
[QueryProperty(nameof(Id), nameof(Id))]
public partial class RatingDetailViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<RatingEditMessage>
{
    public Guid Id { get; set; }
    public RatingDetailModel? Rating { get; private set; }
    public ActivityDetailModel? Activity { get; set; }
    public StudentDetailModel? Student { get; set; }
    public Guid StudentId { get; set; } = Guid.Empty;
    public Guid SubjectId { get; set; } = Guid.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Rating = await ratingFacade.GetAsync(Id);
        if (Rating is null)
        {
            await alertService.DisplayAsync("Error", "Rating was not found or deleted");
            MessengerService.Send(new StudentDeleteMessage());
            navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Rating is not null)
        {
            try
            {
                await ratingFacade.DeleteAsync(Rating.Id);
                MessengerService.Send(new RatingDeleteMessage());
                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("ERROR", "ERROR");
            }
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(RatingEditViewModel.Rating)] = Rating, [nameof(RatingEditViewModel.Activity)] = Activity, [nameof(RatingEditViewModel.SubjectId)] = Activity!.SubjectId });
    }

    public async void Receive(RatingEditMessage message)
    {
        if (message.RatingId == Rating?.Id)
        {
            await LoadDataAsync();
        }
    }
}
