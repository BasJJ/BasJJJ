﻿using NUnit.Framework;
using Moq;
using CoursesManager.UI.ViewModels;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Services;
using CoursesManager.UI.Dialogs.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Dialogs.ResultTypes;
using System.Windows;
using System.Linq;
using CoursesManager.UI.Dialogs.ViewModels;

namespace CoursesManager.Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class DeleteStudentTests
    {
        private Mock<IStudentRepository> _mockStudentRepository;
        private Mock<ICourseRepository> _mockCourseRepository;
        private Mock<IRegistrationRepository> _mockRegistrationRepository;
        private Mock<IDialogService> _mockDialogService;
        private StudentManagerViewModel _viewModel;
        private Student _testStudent;

        [SetUp]
        public void SetUp()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockRegistrationRepository = new Mock<IRegistrationRepository>();
            _mockDialogService = new Mock<IDialogService>();

            _testStudent = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Is_deleted = false
            };

            _mockStudentRepository.Setup(repo => repo.GetAll()).Returns(new List<Student> { _testStudent });

            _viewModel = new StudentManagerViewModel(
                _mockDialogService.Object,
                _mockStudentRepository.Object,
                _mockCourseRepository.Object,
                _mockRegistrationRepository.Object);

            _viewModel.LoadStudents();
        }

        [Test]
        public async Task DeleteStudent_SoftDeletesStudent_WhenConfirmed()
        {
            // Arrange
            _mockDialogService
                .Setup(d => d.ShowDialogAsync<ConfirmationDialogViewModel, DialogResultType>(It.IsAny<DialogResultType>()))
                .ReturnsAsync(DialogResult<DialogResultType>.Builder()
                    .SetSuccess(new DialogResultType { Result = true })
                    .Build());

            _mockDialogService
                .Setup(d => d.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(It.IsAny<DialogResultType>()))
                .ReturnsAsync(DialogResult<DialogResultType>.Builder()
                    .SetSuccess(new DialogResultType())
                    .Build());

            // Act
            await Task.Run(() => _viewModel.DeleteStudentCommand.Execute(_testStudent));

            // Assert
            _mockStudentRepository.Verify(repo => repo.Delete(_testStudent.Id), Times.Once);
            _mockDialogService.Verify(d =>
                d.ShowDialogAsync<ConfirmationDialogViewModel, DialogResultType>(
                    It.Is<DialogResultType>(r => r.DialogText == "Wilt u deze cursist verwijderen?")),
                Times.Once);

            _mockDialogService.Verify(d =>
                d.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(
                    It.Is<DialogResultType>(r => r.DialogText == "Cursist succesvol verwijderd.")),
                Times.Once);
        }

        [Test]
        public async Task DeleteStudent_DoesNotDelete_WhenCancelled()
        {
            // Arrange
            _mockDialogService
                .Setup(d => d.ShowDialogAsync<ConfirmationDialogViewModel, DialogResultType>(It.IsAny<DialogResultType>()))
                .ReturnsAsync(DialogResult<DialogResultType>.Builder()
                    .SetSuccess(new DialogResultType { Result = false })
                    .Build());

            // Act
            await Task.Run(() => _viewModel.DeleteStudentCommand.Execute(_testStudent));

            // Assert
            _mockStudentRepository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
            _mockDialogService.Verify(d =>
                d.ShowDialogAsync<ConfirmationDialogViewModel, DialogResultType>(
                    It.Is<DialogResultType>(r => r.DialogText == "Wilt u deze cursist verwijderen?")),
                Times.Once);

            _mockDialogService.Verify(d =>
                d.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(It.IsAny<DialogResultType>()),
                Times.Never);
        }
    }
}
