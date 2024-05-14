using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Rating;

[QueryProperty(nameof(Rating), nameof(Rating))]
[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class RatingEditViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public RatingDetailModel Rating { get; init; } = RatingDetailModel.Empty;
    public ActivityDetailModel Activity { get; set; }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity is not null)
        {
            await ratingFacade.SaveAsync(Rating with { activityId = Activity.Id, studentId = Guid.Parse(Rating.student) });
        }
        else
        {
            await ratingFacade.SaveAsync(Rating);
        }
        

        MessengerService.Send(new RatingEditMessage { RatingId = Rating.Id });

        navigationService.SendBackButtonPressed();
    }
}
