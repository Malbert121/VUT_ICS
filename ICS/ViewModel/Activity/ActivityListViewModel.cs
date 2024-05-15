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

    private bool _isSearching;

    public bool IsSearching
    {
        get => _isSearching;
        set => SetProperty(ref _isSearching, value);
    }

    [RelayCommand]
    private async Task CancelSearchAsync()
    {
        IsSearching = false;
        await base.LoadDataAsync();

        Activities = await activityFacade.GetFromSubjectAsync(Subject.Id);
    }

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
        await navigationService.GoToAsync("/detail",
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    private async Task SortActivitiesAsync(string sortOption)
    {
        Activities = await activityFacade.GetSortedAsync(sortOption, Subject.Id);
    }

    [RelayCommand]
    private async Task ShowSortOptionsAsync()
    {

        var selectedOption = await App.Current.MainPage.DisplayActionSheet("Sort Activities By", "Cancel", null,
            "byId", "byDescendingId", "byDescendingDate", "byDate", "byDescendingName", "byName", "byDescendingRoom", "byRoom");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            await SortActivitiesAsync(selectedOption);
        }
    }

    [RelayCommand]
    private async Task ShowSearchOptionsAsync()
    {
        var search = await App.Current.MainPage.DisplayPromptAsync("Search", "Enter search term");

        if (!string.IsNullOrEmpty(search))
        {
            await LoadSearchResultsAsync(search);
        }
    }

    [RelayCommand]
    private async Task LoadSearchResultsAsync(string search)
    {
        IsSearching = true;
        Activities = await activityFacade.GetSearchAsync(search, Subject.Id);
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

