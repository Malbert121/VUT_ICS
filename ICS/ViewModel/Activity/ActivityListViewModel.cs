using ICS.Services;
using ICS.Messages;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using ICS.BL.Facade;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ICS.ViewModel.Activity;

[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class ActivityListViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>
{
    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    public SubjectDetailModel Subject { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities = await activityFacade.GetFromSubjectAsync(Subject.Id);
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit", 
            new Dictionary<string, object?> { [nameof(ActivityEditViewModel.Subject)] = Subject });
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }
}

