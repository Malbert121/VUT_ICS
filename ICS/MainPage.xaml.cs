using ICS.View.Rating;
using ICS.View.Activity;
using ICS.ViewModel.Activity;
using ICS.View.Subject;
using ICS.ViewModel.Subject;
namespace ICS;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void OnStudentListClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentListView());
    }

    private void OnRatingListClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RatingListView());
    }

    /* private void OnActivityClicked(object sender, EventArgs e)
     {
         Navigation.PushAsync(new ActivityPage(new ActivityListViewModel));
     }*/
    private void OnSubjectListClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SubjectListView());
    }
}
