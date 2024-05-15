using CommunityToolkit.Mvvm.Input;
using ICS.BL.Facade.Interface;
using ICS.BL.Models;
using ICS.Messages;
using ICS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.ViewModel.Subject
{
    [QueryProperty(nameof(SubjectId), nameof(SubjectId))]
    public partial class SubjectStudentAddViewModel(
        IStudentSubjectFacade studentSubjectFacade,
        IStudentFacade studentFacade,
        IAlertService alertService,
        INavigationService navigationService,
        IMessengerService messengerService)
        : ViewModelBase(messengerService)
    {
        public IEnumerable<StudentListModel> Students { get; set; } = null!;
        public StudentSubjectDetailModel SubjectStudent { get; init; } = StudentSubjectDetailModel.Empty;

        public Guid SubjectId { get; set; }

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();

            Students = await studentFacade.GetAsync();
        }


        [RelayCommand]
        private async Task SelectAsync(Guid id)
        {
            try
            {
                SubjectStudent.StudentId = id;
                await studentSubjectFacade.SaveAsync(SubjectStudent, SubjectId);

                MessengerService.Send(new SubjectStudentAddMessage { SubjectId = SubjectId});

                navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await alertService.DisplayAsync("ERROR", "No Subject or Student was found");
            }
        }
    }
}
