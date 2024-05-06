using ICS.ViewModel.Student;

namespace ICS.View.Student;

public partial class StudentListView
{
    public StudentListView(StudentListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}