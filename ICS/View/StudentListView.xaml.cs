namespace ICS;

public partial class StudentListView
{
	public StudentListView()
	{
		InitializeComponent();
	}

	private void OnEditStudentClicked(object sender, EventArgs e)
	{
        Navigation.PushAsync(new StudentEditProfile());
    }
}