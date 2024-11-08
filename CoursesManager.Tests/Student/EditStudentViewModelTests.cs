using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.ViewModels;
using CoursesManager.MVVM.Dialogs;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using System.Reflection;

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
        _viewModel.StudentCopy.FirstName = "John";
        _viewModel.StudentCopy.LastName = "Doe";
        _viewModel.StudentCopy.Email = "john.doe@example.com";
        _viewModel.StudentCopy.PhoneNumber = "123456789";
        _viewModel.StudentCopy.PostCode = "12345";
        _viewModel.StudentCopy.City = "City";
        _viewModel.StudentCopy.StreetName = "Main Street";
        _viewModel.StudentCopy.HouseNumber = 10;
        _viewModel.StudentCopy.Country = "Country";

        _mockDialogService
            .Setup(service => service.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()))
            .ReturnsAsync(DialogResult<YesNoDialogResultType>.Builder()
                .SetSuccess(new YesNoDialogResultType { Result = true })
                .Build());

        var onSaveAsyncMethod = typeof(EditStudentViewModel).GetMethod("OnSaveAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
        await task;

        _mockStudentRepository.Verify(repo => repo.Update(It.IsAny<Student>()), Times.Once);
        _mockDialogService.Verify(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(It.IsAny<OkDialogResultType>()), Times.Once);
    }

    [Test]
    public async Task OnSaveAsync_MissingFirstName_ShowsOkDialogWithValidationMessage()
    {
        _viewModel.StudentCopy.FirstName = "";
        _viewModel.StudentCopy.LastName = "Doe";
        _viewModel.StudentCopy.Email = "john.doe@example.com";
        _viewModel.StudentCopy.PhoneNumber = "123456789";
        _viewModel.StudentCopy.PostCode = "12345";
        _viewModel.StudentCopy.City = "City";
        _viewModel.StudentCopy.StreetName = "Main Street";
        _viewModel.StudentCopy.HouseNumber = 10;
        _viewModel.StudentCopy.Country = "Country";

        _mockDialogService
            .Setup(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(It.IsAny<OkDialogResultType>()))
            .ReturnsAsync(DialogResult<OkDialogResultType>.Builder()
                .SetSuccess(new OkDialogResultType { Result = true })
                .Build());

        var onSaveAsyncMethod = typeof(EditStudentViewModel).GetMethod("OnSaveAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
        await task;

        _mockStudentRepository.Verify(repo => repo.Update(It.IsAny<Student>()), Times.Never);
        _mockDialogService.Verify(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(
            It.Is<OkDialogResultType>(r => r.DialogText == "Voornaam is verplicht.")), Times.Once);
    }

    [Test]
    public async Task OnSaveAsync_InvalidEmail_ShowsOkDialogWithValidationMessage()
    {
        _viewModel.StudentCopy.FirstName = "John";
        _viewModel.StudentCopy.LastName = "Doe";
        _viewModel.StudentCopy.Email = "invalid-email";
        _viewModel.StudentCopy.PhoneNumber = "123456789";
        _viewModel.StudentCopy.PostCode = "12345";
        _viewModel.StudentCopy.City = "City";
        _viewModel.StudentCopy.StreetName = "Main Street";
        _viewModel.StudentCopy.HouseNumber = 10;
        _viewModel.StudentCopy.Country = "Country";

        _mockDialogService
            .Setup(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(It.IsAny<OkDialogResultType>()))
            .ReturnsAsync(DialogResult<OkDialogResultType>.Builder()
                .SetSuccess(new OkDialogResultType { Result = true })
                .Build());

        var onSaveAsyncMethod = typeof(EditStudentViewModel).GetMethod("OnSaveAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
        await task;

        _mockStudentRepository.Verify(repo => repo.Update(It.IsAny<Student>()), Times.Never);
        _mockDialogService.Verify(service => service.ShowDialogAsync<OkDialogViewModel, OkDialogResultType>(
            It.Is<OkDialogResultType>(r => r.DialogText == "Het opgegeven e-mailadres is ongeldig.")), Times.Once);
    }
}
