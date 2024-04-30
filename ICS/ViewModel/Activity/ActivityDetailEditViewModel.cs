using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Activity;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class IngredientEditViewModel(
    IActivityFacade ingredientFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await ingredientFacade.SaveAsync(Activity);

        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });

        navigationService.SendBackButtonPressed();
    }
}
