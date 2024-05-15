using ICS.BL.Facade.Interface;
using ICS.Services;
using ICS.BL.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICS.Messages;
using ICS.BL.Facade;

namespace ICS.ViewModel.Student;

public partial class StudentListViewModel(
IStudentFacade studentFacade,
INavigationService navigationService,
IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<StudentEditMessage>, IRecipient<StudentDeleteMessage>, IRecipient<StudentAddMessage>
{
    public IEnumerable<StudentListModel> Students { get; set; } = null!;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Students = await studentFacade.GetAsync();
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
        new Dictionary<string, object?> { [nameof(StudentDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    private async Task SortStudentsAsync(string sortOption)
    {
        Students = await studentFacade.GetSortedAsync(sortOption);
    }

    [RelayCommand]
    private async Task ShowSortOptionsAsync()
    {

        var selectedOption = await App.Current.MainPage.DisplayActionSheet("Sort Students By", "Cancel", null,
            "byId", "byDescendingId", "byDescendingLastName", "byLastName");

        if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
        {
            await SortStudentsAsync(selectedOption);
        }
    }

    public async void Receive(StudentEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(StudentDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(StudentAddMessage message)
    {
        await LoadDataAsync();
    }
}
