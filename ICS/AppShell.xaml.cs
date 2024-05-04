using ICS.View;
using ICS.View.Subject;
using System;
using System.Windows.Input;

namespace ICS
{
    public partial class AppShell : Shell
    {
        public ICommand AddStudentCommand { get; }
        public ICommand AddSubjectCommand { get; }
        public AppShell()
        {
            BindingContext = this; // Set AppShell as the BindingContext
            AddStudentCommand = new Command(ExecuteAddStudentCommand);
            AddSubjectCommand = new Command(ExecuteAddSubjectCommand);
            InitializeComponent();
        }

        private async void ExecuteAddStudentCommand()
        {
            // Implement the logic to navigate to the StudentView
            await Shell.Current.Navigation.PushAsync(new StudentAddNew());
        }
        private async void ExecuteAddSubjectCommand()
        {
            await Shell.Current.Navigation.PushAsync(new SubjectAddNew());
        }
    }
}
