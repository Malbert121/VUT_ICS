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
        IAlertService alertService,
        IMessengerService messengerService)
        : ViewModelBase(messengerService)
    {
        public StudentDetailModel Student { get; init; } = StudentDetailModel.Empty;

        [RelayCommand]
        private async Task SaveAsync()
        {
            if(Student.FirstName == string.Empty || Student.LastName == string.Empty)
            {
                await alertService.DisplayAsync("Error", "Some boxes are empty");
                return;
            }

            await studentFacade.SaveAsync(Student with { Subjects = default! });

            MessengerService.Send(new StudentEditMessage { StudentId = Student.Id });

            navigationService.SendBackButtonPressed();
        }
    }

}
