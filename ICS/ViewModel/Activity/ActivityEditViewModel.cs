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

        Activity.start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);

        if (Activity.end < Activity.start)
        {
            Activity.end = Activity.start.AddHours(1);
            EndDate = Activity.end.Date;
            EndTime = Activity.end.TimeOfDay;
        }

    }

    private void UpdateEndDateTime()
    {

        Activity.end = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

        if (Activity.end < Activity.start)
        {
            Activity.start = Activity.end.AddHours(-1);
            StartDate = Activity.start.Date;
            StartTime = Activity.start.TimeOfDay;
        }

    }


    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Activity.name == string.Empty)
        {
            await alertService.DisplayAsync("Error", "Name is empty");
            return;
        }
        if (!(Activity.start == DateTime.MinValue && Activity.end == DateTime.MinValue)
            || Activity.start > Activity.end
            || Activity.start < DateTime.Now)
        {
            await alertService.DisplayAsync("Error", "Start or end time are invalid");
            return;
        }
        if (Activity.room == string.Empty)
        {
            await alertService.DisplayAsync("Error", "Room is empty");
            return;
        }
        if (Subject is not null)
        {
            await activityFacade.SaveAsync(Activity with { subjectId = Subject.Id, ratings = default! });
        }
        else
        {
            await activityFacade.SaveAsync(Activity with { ratings = default! });
        }
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });

        navigationService.SendBackButtonPressed();
    }
}
