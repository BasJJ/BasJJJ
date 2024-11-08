using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.ViewModels;
using CoursesManager.MVVM.Dialogs;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;

[TestFixture]
public class EditStudentViewModelTests
{
    private Mock<IStudentRepository> _mockStudentRepository;
    private Mock<ICourseRepository> _mockCourseRepository;
    private Mock<IRegistrationRepository> _mockRegistrationRepository;
    private Mock<IDialogService> _mockDialogService;
    private EditStudentViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockCourseRepository = new Mock<ICourseRepository>();
        _mockRegistrationRepository = new Mock<IRegistrationRepository>();
        _mockDialogService = new Mock<IDialogService>();

        var student = new Student { FirstName = "Test", LastName = "Student" };

        _viewModel = new EditStudentViewModel(
            _mockStudentRepository.Object,
            _mockCourseRepository.Object,
            _mockRegistrationRepository.Object,
            _mockDialogService.Object,
            student
        );
    }

    [Test]
    public async Task OnSaveAsync_ValidData_ShowsConfirmationDialog()
    {
        // Arrange
        _viewModel.StudentCopy.FirstName = "John";
        _viewModel.StudentCopy.LastName = "Doe";
        _viewModel.StudentCopy.Email = "john.doe@example.com";
        _viewModel.StudentCopy.PostCode = "12345";
        _viewModel.StudentCopy.PhoneNumber = "123456789";

        _mockDialogService
            .Setup(service => service.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()))
            .ReturnsAsync(DialogResult<YesNoDialogResultType>.Builder()
                .SetSuccess(new YesNoDialogResultType { Result = true })
                .Build());

        // Act
        await _viewModel.OnSaveAsync();

        // Assert
        _mockStudentRepository.Verify(repo => repo.Update(It.IsAny<Student>()), Times.Once);
    }

    [Test]
    public async Task OnSaveAsync_InvalidData_ShowsOkDialogWithWarning()
    {
        // Arrange - Create invalid data
        _viewModel.StudentCopy.FirstName = "";
        _viewModel.StudentCopy.Email = "invalid-email";

        _mockDialogService
            .Setup(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(It.IsAny<OkDialogResultType>()))
            .ReturnsAsync(DialogResult<OkDialogResultType>.Builder()
                .SetSuccess(new OkDialogResultType { Result = true })
                .Build());

        // Act
        await _viewModel.OnSaveAsync();

        // Assert
        _mockStudentRepository.Verify(repo => repo.Update(It.IsAny<Student>()), Times.Never);
        _mockDialogService.Verify(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(It.IsAny<OkDialogResultType>()), Times.Once);
    }
}
