using ICS.Views;
using System;
using System.Windows.Input;

namespace ICS
{
    public partial class AppShell : Shell
    {
        public ICommand AddStudentCommand { get; }
        public AppShell()
        {
            BindingContext = this; // Set AppShell as the BindingContext
            AddStudentCommand = new Command(ExecuteAddStudentCommand);
            InitializeComponent();
        }

        private async void ExecuteAddStudentCommand()
        {
            // Implement the logic to navigate to the StudentView
            await Shell.Current.Navigation.PushAsync(new StudentAddNew());
        }
    }
}
