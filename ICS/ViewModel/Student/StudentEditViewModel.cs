using CommunityToolkit.Mvvm.Input;
using ICS.Services;
using ICS.Messages;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;

namespace ICS.ViewModel.Student
{
    [QueryProperty(nameof(Student), nameof(Student))]
    public partial class StudentEditViewModel(
        IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : ViewModelBase(messengerService)
    {
        public StudentDetailModel Student { get; init; } = StudentDetailModel.Empty;

        [RelayCommand]
        private async Task SaveAsync()
        {
            await studentFacade.SaveAsync(Student);

            MessengerService.Send(new StudentEditMessage { StudentId = Student.Id });

            navigationService.SendBackButtonPressed();
        }
    }

}
