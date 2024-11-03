using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using Moq;
using System.Collections.ObjectModel;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using NUnit.Framework.Legacy;

namespace CoursesManager.Tests
{
    [TestFixture]
    public class AddStudentViewModelTests
    {
        private Mock<IStudentRepository> _mockStudentRepository;
        private Mock<ICourseRepository> _mockCourseRepository;
        private Mock<IRegistrationRepository> _mockRegistrationRepository;
        private Mock<ILocationRepository> _mockLocationRepository;

        private TestableAddStudentViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockRegistrationRepository = new Mock<IRegistrationRepository>();
            _mockLocationRepository = new Mock<ILocationRepository>();

            _viewModel = new TestableAddStudentViewModel(_mockStudentRepository.Object, _mockCourseRepository.Object, _mockRegistrationRepository.Object)
            {
                Student = new Student(),
                Courses = new ObservableCollection<string>(_mockCourseRepository.Object.GetAll().Select(c => c.CourseName))
            };
        }

        [Test]
        public void Save_ValidStudent_AddsStudentAndRegistration()
        {
            var course = new Course { ID = 1, CourseName = "Course1" };
            _mockCourseRepository.Setup(repo => repo.GetAll()).Returns(new List<Course> { course });
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "123456789";
            _viewModel.Student.PostCode = "12345";
            _viewModel.Student.HouseNumber = 123;
            _viewModel.Student.City = "asas";
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Once);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Once);
            ClassicAssert.IsTrue(_viewModel.ShowSuccessDialogCalled);
        }

        [Test]
        public void Save_InvalidEmail_ShowsWarningDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "invalid-email";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            ClassicAssert.IsTrue(_viewModel.ShowWarningDialogCalled);

        }

        [Test]
        public void Save_EmptyFirstName_ShowsWarningDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            ClassicAssert.IsTrue(_viewModel.ShowWarningDialogCalled);
        }

        [Test]
        public void Save_EmptyLastName_ShowsWarningDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            ClassicAssert.IsTrue(_viewModel.ShowWarningDialogCalled);
        }

        [Test]
        public void Save_EmptyEmail_ShowsWarningDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            ClassicAssert.IsTrue(_viewModel.ShowWarningDialogCalled);
        }
    }
}
