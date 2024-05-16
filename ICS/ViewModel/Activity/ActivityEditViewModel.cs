using CommunityToolkit.Mvvm.Input;
using ICS.Messages;
using ICS.Services;
using ICS.BL.Models;
using ICS.BL.Facade;
using ICS.BL.Facade.Interface;

namespace ICS.ViewModel.Activity;

[QueryProperty(nameof(Activity), nameof(Activity))]
[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IAlertService alertService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public SubjectDetailModel? Subject { get; init; }

    private DateTime _startDate;
    private TimeSpan _startTime;
    private DateTime _endDate;
    private TimeSpan _endTime;

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (SetProperty(ref _startDate, value))
            {
                UpdateStartDateTime();
            }
        }
    }

    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            if (SetProperty(ref _startTime, value))
            {
                UpdateStartDateTime();
            }
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (SetProperty(ref _endDate, value))
            {
                UpdateEndDateTime();
            }
        }
    }

    public TimeSpan EndTime
    {
        get => _endTime;
        set
        {
            if (SetProperty(ref _endTime, value))
            {
                UpdateEndDateTime();
            }
        }
    }

    private void UpdateStartDateTime()
    {

        Activity.Start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);

        if (Activity.End < Activity.Start)
        {
            Activity.End = Activity.Start.AddHours(1);
            EndDate = Activity.End.Date;
            EndTime = Activity.End.TimeOfDay;
        }

    }

    private void UpdateEndDateTime()
    {

        Activity.End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

        if (Activity.End < Activity.Start)
        {
            Activity.Start = Activity.End.AddHours(-1);
            StartDate = Activity.Start.Date;
            StartTime = Activity.Start.TimeOfDay;
        }

    }


    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity.Name == string.Empty)
        {
            await alertService.DisplayAsync("Error", "Name is empty");
            return;
        }
        if (!(Activity.Start == DateTime.MinValue && Activity.End == DateTime.MinValue)
            || Activity.Start > Activity.End
            || Activity.Start < DateTime.Now)
        {
            await alertService.DisplayAsync("Error", "Start or End time are invalid");
            return;
        }
        if (Activity.Room == string.Empty)
        {
            await alertService.DisplayAsync("Error", "Room is empty");
            return;
        }
        if (Subject is not null)
        {
            await activityFacade.SaveAsync(Activity with { SubjectId = Subject.Id, Ratings = default! });
        }
        else
        {
            await activityFacade.SaveAsync(Activity with { Ratings = default! });
        }
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });

        navigationService.SendBackButtonPressed();
    }
}
