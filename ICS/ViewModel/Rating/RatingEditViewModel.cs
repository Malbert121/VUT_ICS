using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Rating;

[QueryProperty(nameof(Rating), nameof(Rating))]
public partial class RatingEditViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public RatingDetailModel Rating { get; init; } = RatingDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await ratingFacade.SaveAsync(Rating);

        MessengerService.Send(new RatingEditMessage { RatingId = Rating.Id });

        navigationService.SendBackButtonPressed();
    }
}
