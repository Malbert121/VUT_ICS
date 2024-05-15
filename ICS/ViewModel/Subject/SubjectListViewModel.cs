using ICS.Services;
using ICS.Messages.SubjectMessages;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICS.BL.Facade;

namespace ICS.ViewModel.Subject;

public partial class SubjectListViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>, IRecipient<SubjectAddMessage>
{
    public IEnumerable<SubjectListModel> Subjects { get; set; } = null!;

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

         Subjects = await subjectFacade.GetAsync();
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subjects = await subjectFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("/detail",
            new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    private async Task SortSubjectsAsync(string sortOption)
    {
        Subjects = await subjectFacade.GetSortedAsync(sortOption);
    }

    [RelayCommand]
    private async Task ShowSortOptionsAsync()
    {

        var selectedOption = await App.Current.MainPage.DisplayActionSheet("Sort Subjects By", "Cancel", null,
            "byId", "byDescendingId", "byDescendingName", "byName");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            await SortSubjectsAsync(selectedOption);
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
        Subjects = await subjectFacade.GetSearchAsync(search);
    }

    public async void Receive(SubjectEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(SubjectDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(SubjectAddMessage message)
    {
        await LoadDataAsync();
    }
}
