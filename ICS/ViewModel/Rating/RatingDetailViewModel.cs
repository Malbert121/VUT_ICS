using ICS.Services;
using ICS.Messages;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ICS.ViewModel.Rating;

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

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Rating = await ratingFacade.GetAsync(Id);
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
            new Dictionary<string, object?> { [nameof(RatingEditViewModel.Rating)] = Rating });
    }

    public async void Receive(RatingEditMessage message)
    {
        if (message.RatingId == Rating?.Id)
        {
            await LoadDataAsync();
        }
    }
}
