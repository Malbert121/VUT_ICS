using CommunityToolkit.Mvvm.Input;
using ICS.Services;
using ICS.View;
using ICS.ViewModel.Activity;
using ICS.ViewModel.Rating;
using ICS.ViewModel.Subject;
using System;
using System.Linq.Expressions;
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
        {
            
            await _navigationService.GoToAsync<ActivityListViewModel>();
        }

        [RelayCommand]
        private async Task GoToSubjectsAsync()
        => await _navigationService.GoToAsync<SubjectListViewModel>();
            
            
            
        
    }
   
}
