using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using Moq;
using System.Collections.ObjectModel;
using CoursesManager.UI.Models.CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.LocationRepository;

namespace CoursesManager.Tests
{
    [TestFixture]
    public class AddStudentViewModelTests
    {
        private Mock<IStudentRepository> _mockStudentRepository;
        private Mock<ICourseRepository> _mockCourseRepository;
        private Mock<IRegistrationRepository> _mockRegistrationRepository;
        private Mock<ILocationRepository> _mockLocationnRepository;

        private AddStudentViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockRegistrationRepository = new Mock<IRegistrationRepository>();
            _mockLocationnRepository = new Mock<ILocationRepository>();

            _viewModel = new AddStudentViewModel(_mockStudentRepository.Object, _mockCourseRepository.Object, _mockRegistrationRepository.Object)
            {
                Student = new Student(),
                Courses = new ObservableCollection<string>(_mockCourseRepository.Object.GetAll().Select(c => c.CourseName))
            };
        }

        //better to allow it when we have database
  /*      [Test]
        public void Save_ValidStudent_AddsStudentAndRegistration()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.Student.HouseNumber = 123;
            _viewModel.SelectedCourse = "Course1";

            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Once);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Once);
        }*/

        [Test]
        public void Save_InvalidEmail_DoesNotAddStudent()
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
        }

        [Test]
        public void Save_EmptyFirstName_ThrowsException()
        {
            // Arrange
            _viewModel.Student.FirstName = "";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act & Assert
            Assert.Throws<Exception>(() => _viewModel.SaveCommand.Execute(null));
        }

        [Test]
        public void Save_EmptyLastName_ThrowsException()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act & Assert
            Assert.Throws<Exception>(() => _viewModel.SaveCommand.Execute(null));
        }

        [Test]
        public void Save_EmptyEmail_ThrowsException()
        {
            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            // Act & Assert
            var caughtException = Assert.Throws<Exception>(() => _viewModel.SaveCommand.Execute(null));
            Assert.That(caughtException.Message, Is.EqualTo("Invalid student data"));
        }
    }
}
