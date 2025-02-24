﻿using CommunityToolkit.Mvvm.Input;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.Services;
using ICS.Messages;
using ICS.Messages.SubjectMessages;
using CommunityToolkit.Mvvm.Messaging;

namespace ICS.ViewModel.Subject;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectStudentViewModel(
    IStudentSubjectFacade studentSubjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<SubjectStudentAddMessage>
{
    public Guid Id { get; set; }

    public Guid SubjectId { get; set; }
    public IEnumerable<StudentSubjectListModel> Students { get; set; } = null!;

    private bool _wasModified;

    public bool wasModified
    {
        get => _wasModified;
        set => SetProperty(ref _wasModified, value);
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Students = await studentSubjectFacade.GetStudentsAsync(Id);
    }

    [RelayCommand]
    private async Task GoToAddAsync()
    {
        await navigationService.GoToAsync("/add",
        new Dictionary<string, object?> { [nameof(SubjectStudentAddViewModel.SubjectId)] = SubjectId });
    }

    [RelayCommand]
    private async Task DeleteAsync(Guid id)
    {
        try
        {
            await studentSubjectFacade.DeleteAsync(id);
            MessengerService.Send(new StudentSubjectDeleteMessage());
            await LoadDataAsync();

        }
        catch (InvalidOperationException)
        {
            await alertService.DisplayAsync("ERROR", "No Subject or Student was found");
        }
    }

    [RelayCommand]
    private async Task CancelSearchAsync()
    {
        wasModified = false;
        await base.LoadDataAsync();

        Students = await studentSubjectFacade.GetAsync();
    }

    [RelayCommand]
    private async Task SortStudentsAsync(string sortOption)
    {
        try
        {
            wasModified = true;
            Students = await studentSubjectFacade.GetSortedAsync(sortOption, SubjectId);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"An error occurred while sorting students: {ex.Message}", "OK");
        }
    }


    [RelayCommand]
    private async Task ShowSortOptionsAsync()
    {

        var selectedOption = await Application.Current!.MainPage!.DisplayActionSheet("Sort Students By", "Cancel", null,
            "byId", "byDescendingId", "byDescendingLastName", "byLastName");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            await SortStudentsAsync(selectedOption);
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
        try
        {
            wasModified = true;
            Students = await studentSubjectFacade.GetSearchAsync(search, SubjectId);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"An error occurred while searching students: {ex.Message}", "OK");
        }
    }


    public async void Receive(SubjectStudentAddMessage message)
    {
       await LoadDataAsync();
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
