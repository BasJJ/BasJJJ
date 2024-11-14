using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using Moq;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Services;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using CoursesManager.UI.Helpers;
using System.Windows.Controls;

// Temporary should be fixed next sprint
namespace CoursesManager.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class AddStudentViewModelTests
    {
        private Mock<IStudentRepository> _mockStudentRepository;
        private Mock<ICourseRepository> _mockCourseRepository;
        private Mock<IRegistrationRepository> _mockRegistrationRepository;
        private Mock<IDialogService> _mockDialogService;
        private AddStudentViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockRegistrationRepository = new Mock<IRegistrationRepository>();
            _mockDialogService = new Mock<IDialogService>();

            _viewModel = new AddStudentViewModel(true,
                _mockStudentRepository.Object,
                _mockCourseRepository.Object,
                _mockRegistrationRepository.Object,
                _mockDialogService.Object)
            {
                Student = new Student(),
                Courses = new ObservableCollection<string>(_mockCourseRepository.Object.GetAll().Select(c => c.Name)),
                ParentWindow = new Window()
            };

     /*       var grid = new Grid();
            var firstNameTextBox = new TextBox { Tag = "Voornaam" };
            ValidationProperties.SetIsRequired(firstNameTextBox, true);
            grid.Children.Add(firstNameTextBox);

            var tussenvoegselTextBox = new TextBox { Tag = "`Tussenvoegsel" };
            ValidationProperties.SetIsRequired(tussenvoegselTextBox, false);
            grid.Children.Add(tussenvoegselTextBox);

            var lastNameTextBox = new TextBox { Tag = "Achternaam" };
            ValidationProperties.SetIsRequired(lastNameTextBox, true);
            grid.Children.Add(lastNameTextBox);

            var emailTextBox = new TextBox { Tag = "Email" };
            ValidationProperties.SetIsRequired(emailTextBox, true);
            ValidationProperties.SetIsEmail(emailTextBox, true);
            grid.Children.Add(emailTextBox);

            var phoneNumberTextBox = new TextBox { Tag = "Telefoonnummer" };
            ValidationProperties.SetIsRequired(phoneNumberTextBox, true);
            ValidationProperties.SetIsPhoneNumber(phoneNumberTextBox, true);
            grid.Children.Add(phoneNumberTextBox);

            var postCodeTextBox = new TextBox { Tag = "Postcode" };
            ValidationProperties.SetIsRequired(postCodeTextBox, true);
            grid.Children.Add(postCodeTextBox);

            var houseNumberTextBox = new TextBox { Tag = "Huisnummer" };
            ValidationProperties.SetIsRequired(houseNumberTextBox, true);
            grid.Children.Add(houseNumberTextBox);

            var streetNameTextBox = new TextBox { Tag = "Straatnaam" };
            ValidationProperties.SetIsRequired(streetNameTextBox, true);
            grid.Children.Add(streetNameTextBox);

            var countryTextBox = new TextBox { Tag = "Land" };
            ValidationProperties.SetIsRequired(countryTextBox, true);
            grid.Children.Add(postCodeTextBox);

            var cityTextBox = new TextBox { Tag = "Stand" };
            ValidationProperties.SetIsRequired(cityTextBox, true);
            grid.Children.Add(cityTextBox);

            _viewModel.ParentWindow.Content = null;
            _viewModel.ParentWindow.Content = grid;*/
        }

        [Test]
        public async Task Save_EmptyFirstName_ShowsConfirmationDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "";
            _viewModel.Student.Insertion = "";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.HouseNumber = 15;
            _viewModel.Student.City = "DemoLand";
            _viewModel.Student.Country = "DemoCountry";
            _viewModel.Student.StreetName = "DemoStreet";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            _mockDialogService
                .Setup(service => service.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()))
                .ReturnsAsync(DialogResult<ConfirmationDialogResultType>.Builder()
                    .SetSuccess(new ConfirmationDialogResultType { Result = true })
                    .Build());

            var onSaveAsyncMethod = typeof(AddStudentViewModel).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Instance);
            if (onSaveAsyncMethod != null)
            {
                var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
                if (task != null)
                {
                    await task;
                }
            }

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            _mockDialogService.Verify(ds => ds.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>
                (It.IsAny<ConfirmationDialogResultType>()), Times.Once);
        }

        [Test]
        public async Task  Save_EmptyLastName_ShowsConfirmationDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "Doe";
            _viewModel.Student.Insertion = "";
            _viewModel.Student.LastName = "";
            _viewModel.Student.Email = "john.doe@example.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.HouseNumber = 15;
            _viewModel.Student.City = "DemoLand";
            _viewModel.Student.Country = "DemoCountry";
            _viewModel.Student.StreetName = "DemoStreet";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            _mockDialogService
                .Setup(service => service.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()))
                .ReturnsAsync(DialogResult<ConfirmationDialogResultType>.Builder()
                    .SetSuccess(new ConfirmationDialogResultType { Result = true })
                    .Build());

            // Act
            var onSaveAsyncMethod = typeof(AddStudentViewModel).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Instance);
            if (onSaveAsyncMethod != null)
            {
                var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
                if (task != null)
                {
                    await task;
                }
            }
            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            _mockDialogService.Verify(ds => ds.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()), Times.Once);
        }

        [Test]
        public async Task Save_InvalidEmail_ShowsConfirmationDialog()
        {
            // Arrange
            _viewModel.Student.FirstName = "Doe";
            _viewModel.Student.Insertion = "";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john.doe.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.HouseNumber = 15;
            _viewModel.Student.City = "DemoLand";
            _viewModel.Student.Country = "DemoCountry";
            _viewModel.Student.StreetName = "DemoStreet";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            _mockDialogService
                .Setup(service => service.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()))
                .ReturnsAsync(DialogResult<ConfirmationDialogResultType>.Builder()
                    .SetSuccess(new ConfirmationDialogResultType { Result = true })
                    .Build());

            // Act
            var onSaveAsyncMethod = typeof(AddStudentViewModel).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Instance);
            if (onSaveAsyncMethod != null)
            {
                var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
                if (task != null)
                {
                    await task;
                }
            }
            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Never);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Never);
            _mockDialogService.Verify(service => service.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(It.IsAny<ConfirmationDialogResultType>()), Times.Once);
        }

        [Test]
        public async Task Save_ValidStudent_AddsStudentAndRegistration()
        {
            var course = new Course { ID = 1, Name = "Course1" };
            _mockCourseRepository.Setup(repo => repo.GetAll()).Returns(new List<Course> { course });

            // Arrange
            _viewModel.Student.FirstName = "John";
            _viewModel.Student.Insertion = "";
            _viewModel.Student.LastName = "Doe";
            _viewModel.Student.Email = "john@doe.com";
            _viewModel.Student.PhoneNumber = "1234567890";
            _viewModel.Student.HouseNumber = 15;
            _viewModel.Student.City = "DemoLand";
            _viewModel.Student.Country = "DemoCountry";
            _viewModel.Student.StreetName = "DemoStreet";
            _viewModel.Student.PostCode = "12345";
            _viewModel.SelectedCourse = "Course1";

            _mockDialogService
                .Setup(service => service.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()))
                .ReturnsAsync(DialogResult<YesNoDialogResultType>.Builder()
                    .SetSuccess(new YesNoDialogResultType { Result = true })
                    .Build());

            // Act
            var onSaveAsyncMethod = typeof(AddStudentViewModel).GetMethod("Save", BindingFlags.NonPublic | BindingFlags.Instance);
            if (onSaveAsyncMethod != null)
            {
                var task = (Task)onSaveAsyncMethod.Invoke(_viewModel, null);
                if (task != null)
                {
                    await task;
                }
            }

            // Assert
            _mockStudentRepository.Verify(repo => repo.Add(It.IsAny<Student>()), Times.Once);
            _mockRegistrationRepository.Verify(repo => repo.Add(It.IsAny<Registration>()), Times.Once);
            _mockDialogService.Verify(ds => ds.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(It.IsAny<YesNoDialogResultType>()), Times.Once);
        }
    }
}