using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.Messages;
using ICS.Messages.SubjectMessages;
using ICS.Services;
using ICS.ViewModel.Subject;


namespace ICS.ViewModel.Student;
[QueryProperty(nameof(Id), nameof(Id))]
public partial class StudentSubjectViewModel(
    IStudentSubjectFacade studentSubjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    public IEnumerable<StudentSubjectListModel> Subjects { get; set; } = null!;


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subjects = await studentSubjectFacade.GetSubjectsAsync(Id);
    }

}
