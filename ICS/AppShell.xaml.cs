using CommunityToolkit.Mvvm.Input;
using ICS.Services;
using ICS.View;
using ICS.ViewModel.Activity;
using ICS.ViewModel.Rating;
using ICS.ViewModel.Student;
using System;
using System.Windows.Input;

namespace ICS
{
    public partial class AppShell : Shell
    {
        private readonly INavigationService _navigationService;
        public AppShell(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BindingContext = this; // Set AppShell as the BindingContext
            InitializeComponent();
        }

        [RelayCommand]
        private async Task GoToRatingsAsync()
        => await _navigationService.GoToAsync<RatingListViewModel>();

        [RelayCommand]
        private async Task GoToActivitiesAsync()
        => await _navigationService.GoToAsync<ActivityListViewModel>();

        [RelayCommand]
        private async Task GoToStudentsAsync()
        => await _navigationService.GoToAsync<StudentListViewModel>();

    }
   
}
