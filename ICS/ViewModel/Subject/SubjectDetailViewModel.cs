using ICS.Services;
using ICS.Messages.SubjectMessages;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICS.ViewModel.Activity;


namespace ICS.ViewModel.Subject;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class SubjectDetailViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>
{

    public SubjectDetailModel? Subject { get; private set; }
    public Guid Id { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subject = await subjectFacade.GetAsync(Id);

        if (Subject is null)
        {
            await alertService.DisplayAsync("Error", "Subject was not found or deleted");
            MessengerService.Send(new SubjectDeleteMessage());
            navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Subject is not null)
        {
            try
            {
                await subjectFacade.DeleteAsync(Subject.Id);
                MessengerService.Send(new SubjectDeleteMessage());
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
            new Dictionary<string, object?> { [nameof(SubjectEditViewModel.Subject)] = Subject });
    }

    [RelayCommand]
    private async Task GoToActivityAsync()
    {
        if(Subject is not null)
        {
            await navigationService.GoToAsync("/activities",
        new Dictionary<string, object?> { [nameof(ActivityListViewModel.Activities)] = Subject.activity, [nameof(ActivityListViewModel.Subject)] = Subject });
        }
        else
        {
            await alertService.DisplayAsync("ERROR", "No Subject was found");
        }
        
    }

    [RelayCommand]
    private async Task GoToStudentsAsync()
    {
        if(Subject is not null)
        {
            await navigationService.GoToAsync("/students",
            new Dictionary<string, object?> { [nameof(SubjectStudentViewModel.Id)] = Id, [nameof(SubjectStudentViewModel.SubjectId)] = Subject.Id });
        }
        else
        {
            await alertService.DisplayAsync("ERROR", "No Subject was found");
        }
    }

    public async void Receive(SubjectEditMessage message)
    {
        if (message.SubjectId == Subject?.Id)
        {
            await LoadDataAsync();
        }
    }
}
