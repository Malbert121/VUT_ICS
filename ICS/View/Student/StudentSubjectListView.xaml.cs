using ICS.View.Subject;
using ICS.ViewModel.Subject;

namespace ICS.View.Student;

public partial class StudentSubjectView
{
    public StudentSubjectView(SubjectListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}