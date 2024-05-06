using CommunityToolkit.Mvvm.Input;
using ICS.Messages.SubjectMessages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Subject;

[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class SubjectEditViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public SubjectDetailModel Subject { get; init; } = SubjectDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        
        await subjectFacade.SaveAsync(Subject with { activity = default!, students = default! });
       
        MessengerService.Send(new SubjectEditMessage { SubjectId = Subject.Id });

        navigationService.SendBackButtonPressed();
    }
}
