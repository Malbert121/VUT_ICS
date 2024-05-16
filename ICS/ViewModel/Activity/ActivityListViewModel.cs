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

    public SubjectDetailModel? Subject { get; set; }

    private bool _wasModified;

    public bool wasModified
    {
        get => _wasModified;
        set => SetProperty(ref _wasModified, value);
    }

    [RelayCommand]
    private async Task CancelSearchAsync()
    {
        wasModified = false;
        await base.LoadDataAsync();

        Activities = await activityFacade.GetFromSubjectAsync(Subject!.Id);
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities = await activityFacade.GetFromSubjectAsync(Subject!.Id);
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
        wasModified = true;
        Activities = await activityFacade.GetSortedAsync(sortOption, Subject!.Id);
    }

    [RelayCommand]
    private async Task ShowSortOptionsAsync()
    {

        var selectedOption = await Application.Current!.MainPage!.DisplayActionSheet("Sort Activities By", "Cancel", null,
            "byId", "byDescendingId", "byDescendingDate", "byDate", "byDescendingName", "byName", "byDescendingRoom", "byRoom");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            await SortActivitiesAsync(selectedOption);
        }
    }

    [RelayCommand]
    private async Task ShowSearchOptionsAsync()
    {
        var search = await Application.Current!.MainPage!.DisplayPromptAsync("Search", "Enter search term");

        if (!string.IsNullOrEmpty(search))
        {
            await LoadSearchResultsAsync(search);
        }
    }

    [RelayCommand]
    private async Task LoadSearchResultsAsync(string search)
    {
        wasModified = true;
        Activities = await activityFacade.GetSearchAsync(search, Subject!.Id);
    }


    [RelayCommand]
    private async Task ShowFilteringOptionsAsync()
    {
        var selectedOption = await Application.Current!.MainPage!.DisplayActionSheet("Filter Activities by", "Cancel", null,
                "by Start Date", "by Start and End Date");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            if (selectedOption == "by Start Date")
            {
                var startDate = await Application.Current!.MainPage!.DisplayPromptAsync("Filter by Start Date", "Enter Start date (DD-MM-YYYY hh:mm:ss tt)");
                if (startDate != null)
                {
                    if (DateTime.TryParse(startDate, out DateTime start))
                    {
                        wasModified = true;
                        Activities = await activityFacade.GetFilteredAsync(Subject!.Id, start);
                    }
                    else
                    {
                        await Application.Current!.MainPage!.DisplayAlert("Error", "Invalid date format entered.", "OK");
                    }
                }
            }
            else if (selectedOption == "by Start and End Date")
            {
                var startDate = await Application.Current!.MainPage!.DisplayPromptAsync("Filter by Start Date", "Enter Start date (DD-MM-YYYY)");
                var endDate = await Application.Current!.MainPage!.DisplayPromptAsync("Filter by Start and End Date", "Enter End date (DD-MM-YYYY)");
                if (startDate != null && endDate != null)
                {
                    if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end))
                    {
                        wasModified = true;
                        Activities = await activityFacade.GetFilteredAsync(Subject!.Id, start, end);
                    }
                    else
                    {
                        await Application.Current!.MainPage!.DisplayAlert("Error", "Invalid date format entered.", "OK");
                    }
                }
            }
        }
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

