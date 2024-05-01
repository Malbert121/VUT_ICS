
using CommunityToolkit.Mvvm.Input;
using ICS.Services;
using ICS.Messages;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.Models;

namespace ICS.ViewModel.Student
{

    public partial class StudentDetailEditViewModel : ViewModelBase
    {
        private readonly IStudentFacade _studentFacade;
        private readonly INavigationService _navigationService;
        private readonly IMessengerService _messengerService;

        public StudentDetailEditViewModel(IStudentFacade studentFacade, INavigationService navigationService, IMessengerService messengerService)
            : base(messengerService)
        {
            _studentFacade = studentFacade;
            _navigationService = navigationService;
            _messengerService = messengerService;
        }

        public StudentDetailModel Student { get; set; } = StudentDetailModel.Empty;

        [RelayCommand]
        private async Task SaveChanges()
        {
            await _studentFacade.SaveAsync(Student);

            _messengerService.Send(new StudentEditMessage { StudentId = Student.Id });

            _navigationService.SendBackButtonPressed();
        }
    }   


}
