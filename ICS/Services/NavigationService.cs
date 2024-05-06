using ICS.Models;
using ICS.ViewModel.Activity;
using ICS.ViewModel;
using ICS.View.Rating;
using ICS.ViewModel.Rating;
using ICS.View.Activity;
using ICS.View.Subject;
using ICS.ViewModel.Subject;
using ICS.View.Student;
using ICS.ViewModel.Student;

namespace ICS.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//ratings", typeof(RatingListView), typeof(RatingListViewModel)),
        new("//ratings/detail", typeof(RatingDetailView), typeof(RatingDetailViewModel)),
        new("//ratings/edit", typeof(RatingEditView), typeof(RatingEditViewModel)),
        new("//ratings/detail/edit", typeof(RatingEditView), typeof(RatingEditViewModel)),

        new("//activities", typeof(ActivityListView), typeof(ActivityListViewModel)),
        new("//activities/detail", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
        new("//activities/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),
        new("//activities/detail/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),

        new("//subjects", typeof(SubjectListView), typeof(SubjectListViewModel)),
        new("//subjects/detail", typeof(SubjectDetailView), typeof(SubjectDetailViewModel)),
        new("//subjects/edit", typeof(SubjectEditView), typeof(SubjectEditViewModel)),
        new("//subjects/detail/edit", typeof(SubjectEditView), typeof(SubjectEditViewModel)),

        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//students/detail", typeof(StudentDetailView), typeof(StudentDetailViewModel)),
        new("//students/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//students/detail/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),

    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }
    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}
