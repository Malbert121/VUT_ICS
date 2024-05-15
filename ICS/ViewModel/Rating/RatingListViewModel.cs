using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade.Interface;
using ICS.ViewModel.Activity;


namespace ICS.ViewModel.Rating
{
    [QueryProperty(nameof(Activity), nameof(Activity))]
    public partial class RatingListViewModel(
        IRatingFacade ratingFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : ViewModelBase(messengerService), IRecipient<RatingEditMessage>, IRecipient<RatingDeleteMessage>, IRecipient<RatingAddMessage>
    {
        public IEnumerable<RatingListModel> Ratings { get; set; } = null!;
        public ActivityDetailModel Activity { get; set; }

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();

            Ratings = await ratingFacade.GetFromActivityAsync(Activity.Id);
        }

        [RelayCommand]
        private async Task GoToCreateAsync()
        {
            await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(RatingEditViewModel.Activity)] = Activity, [nameof(RatingEditViewModel.SubjectId)] = Activity.subjectId });
        }

        [RelayCommand]
        private async Task GoToDetailAsync(Guid id)
        {
            await navigationService.GoToAsync("/detail",
              new Dictionary<string, object?> { [nameof(RatingDetailViewModel.Id)] = id, [nameof(RatingEditViewModel.Activity)] = Activity, [nameof(RatingEditViewModel.SubjectId)] = Activity.subjectId });
        }

        [RelayCommand]
        private async Task SortRatingsAsync(string sortOption)
        {
            Ratings = await ratingFacade.GetSortedAsync(sortOption, Activity.Id);
        }

        [RelayCommand]
        private async Task ShowSortOptionsAsync()
        {

            var selectedOption = await App.Current.MainPage.DisplayActionSheet("Sort Ratings By", "Cancel", null,
                "byId", "byDescendingId", "byDescendingPoints", "byPoints");

            if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Cancel")
            {
                await SortRatingsAsync(selectedOption);
            }
        }

        [RelayCommand]
        private async Task ShowSearchOptionsAsync()
        {
            var search = await App.Current.MainPage.DisplayPromptAsync("Search", "Enter search term");

            if (!string.IsNullOrEmpty(search))
            {
                await LoadSearchResultsAsync(search);
            }
        }

        private async Task LoadSearchResultsAsync(string search)
        {
            Ratings = await ratingFacade.GetSearchAsync(search, Activity.Id);
        }

        public async void Receive(RatingEditMessage message)
        {
            await LoadDataAsync();
        }

        public async void Receive(RatingDeleteMessage message)
        {
            await LoadDataAsync();
        }

        public async void Receive(RatingAddMessage message)
        {
            await LoadDataAsync();
        }
    }

}
