using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;using CoursesManager.UI.ViewModels;

public class TestableAddStudentViewModel : AddStudentViewModel
{
    public bool ShowWarningDialogCalled { get; private set; }
    public bool ShowSuccessDialogCalled { get; private set; }


    public TestableAddStudentViewModel(bool initial, IStudentRepository studentRepository,
        ICourseRepository courseRepository, IRegistrationRepository registrationRepository,
        IDialogService dialogService
    )
        : base(initial, studentRepository, courseRepository, registrationRepository, dialogService)
    {
    }
    protected override void ShowWarningDialog(DialogResult<bool> dialogResult)
    {
        ShowWarningDialogCalled = true;
    }
    protected override void ShowSuccessDialog(DialogResult<bool> dialogResult)
    {
        if (dialogResult.Outcome == DialogOutcome.Success)
        {
            ShowSuccessDialogCalled = true;
        }
    }
}
