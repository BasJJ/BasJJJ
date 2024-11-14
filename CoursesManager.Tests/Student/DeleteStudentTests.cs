using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.ViewModels;
using CoursesManager.MVVM.Dialogs;
using Moq;
using NUnit.Framework;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI;
using System.Reflection;

namespace CoursesManager.Tests
{
    [TestFixture]
    public class DeleteStudentTests
    {
        private Mock<IDialogService> _mockDialogService;
        private StudentManagerViewModel _viewModel;
        private Student _testStudent;

        [SetUp]
        public void SetUp()
        {
            var studentsProperty = typeof(App).GetProperty("Students", BindingFlags.Static | BindingFlags.Public);
            studentsProperty.SetValue(null, new ObservableCollection<Student>());

            _mockDialogService = new Mock<IDialogService>();

            _testStudent = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Is_deleted = false
            };

            App.Students.Add(_testStudent);

            _viewModel = new StudentManagerViewModel(_mockDialogService.Object);

            _viewModel.students = App.Students;
            _viewModel.FilteredStudentRecords = new ObservableCollection<Student>(App.Students);
        }

        [Test]
        public async Task DeleteStudent_SoftDeletesStudent_WhenConfirmed()
        {
            _mockDialogService
                .Setup(d => d.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()))
                .ReturnsAsync(DialogResult<YesNoDialogResultType>.Builder()
                    .SetSuccess(new YesNoDialogResultType { Result = true })
                    .Build());

            _mockDialogService
                .Setup(d => d.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()))
                .ReturnsAsync(DialogResult<ConfirmationDialogResultType>.Builder()
                    .SetSuccess(new ConfirmationDialogResultType())
                    .Build());

            _viewModel.DeleteStudentCommand.Execute(_testStudent);

            Assert.That(_testStudent.Is_deleted, Is.True, "Student should be marked as deleted.");
            Assert.That(_testStudent.date_deleted?.Date, Is.EqualTo(DateTime.Now.Date), "The deletion date should be set to today's date.");

            _mockDialogService.Verify(d => d.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()), Times.Once);
        }

        [Test]
        public async Task DeleteStudent_DoesNotDelete_WhenCancelled()
        {
            _mockDialogService
                .Setup(d => d.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()))
                .ReturnsAsync(DialogResult<YesNoDialogResultType>.Builder()
                    .SetSuccess(new YesNoDialogResultType { Result = false })
                    .Build());

            _viewModel.DeleteStudentCommand.Execute(_testStudent);

            Assert.That(_testStudent.Is_deleted, Is.False, "Student should initially not be marked as deleted.");
            Assert.That(_testStudent.date_deleted, Is.Null, "Student's deletion date should initially be null.");

            _mockDialogService.Verify(d => d.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()), Times.Never);
        }
    }
}
