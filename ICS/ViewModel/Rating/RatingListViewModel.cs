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

    public partial class RatingListViewModel(
        IRatingFacade ratingFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : ViewModelBase(messengerService), IRecipient<RatingEditMessage>, IRecipient<RatingDeleteMessage>, IRecipient<RatingAddMessage>
    {
        public IEnumerable<RatingListModel> Ratings { get; set; } = null!;

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();

            Ratings = await ratingFacade.GetAsync();
        }

        [RelayCommand]
        private async Task GoToCreateAsync()
        {
            await navigationService.GoToAsync("/edit");
        }

        [RelayCommand]
        private async Task GoToDetailAsync(Guid id)
        {
            //await navigationService.GoToAsync<ActivityDetailViewModel>(
            //  new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
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
