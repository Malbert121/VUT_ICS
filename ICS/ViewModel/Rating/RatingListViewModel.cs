﻿using CommunityToolkit.Mvvm.Input;
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
            new Dictionary<string, object?> { [nameof(RatingEditViewModel.Activity)] = Activity });
        }

        [RelayCommand]
        private async Task GoToDetailAsync(Guid id)
        {
            await navigationService.GoToAsync<RatingDetailViewModel>(
              new Dictionary<string, object?> { [nameof(RatingDetailViewModel.Id)] = id });
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
