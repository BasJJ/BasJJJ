﻿using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.AddressRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.ViewModels;
using CoursesManager.UI.ViewModels.Courses;
using CoursesManager.UI.ViewModels.Students;

namespace CoursesManager.UI.Factory
{
    public class ViewModelFactory
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDialogService _dialogService;
        public readonly INavigationService _navigationService;

        public ViewModelFactory(
            ICourseRepository courseRepository,
            ILocationRepository locationRepository,
            IRegistrationRepository registrationRepository,
            IStudentRepository studentRepository,
            IAddressRepository addressRepository,
            IMessageBroker messageBroker,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _courseRepository = courseRepository;
            _locationRepository = locationRepository;
            _registrationRepository = registrationRepository;
            _studentRepository = studentRepository;
            _addressRepository = addressRepository;
            _messageBroker = messageBroker;
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        public T CreateViewModel<T>(object parameter = null) where T : class
        {
            return typeof(T) switch
            {
                Type vmType when vmType == typeof(StudentManagerViewModel) =>
                    new StudentManagerViewModel(_dialogService, _studentRepository, _courseRepository,
                        _registrationRepository, _messageBroker, _navigationService) as T,
                Type vmType when vmType == typeof(CourseOverViewViewModel) =>
                    new CourseOverViewViewModel() as T,
                Type vmType when vmType == typeof(EditStudentViewModel) =>
                    new EditStudentViewModel(
                        _studentRepository,
                        _courseRepository,
                        _registrationRepository,
                        _dialogService,
                        parameter as Student) as T,
                Type vmType when vmType == typeof(AddStudentViewModel) =>
                    new AddStudentViewModel(false, _studentRepository, _courseRepository, _registrationRepository,
                        _dialogService) as T,
                Type vmType when vmType == typeof(StudentDetailViewModel) =>
                    new StudentDetailViewModel(_dialogService,_messageBroker,_navigationService,_registrationRepository,_courseRepository, _studentRepository, parameter as Student)
                     as T,

                _ => throw new ArgumentException($"Unknown ViewModel type: {typeof(T)}")
            };
        }

        public T CreateViewModel<T>(INavigationService navigationService, object parameter = null)
            where T : NavigatableViewModel
        {
            return typeof(T) switch
            {
                Type vmType when vmType == typeof(CoursesManagerViewModel) =>
                    new CoursesManagerViewModel(_courseRepository, _registrationRepository, navigationService, _messageBroker) as T,
                Type vmType when vmType == typeof(StudentDetailViewModel) =>
                    new StudentDetailViewModel(_dialogService,_messageBroker,_navigationService,_registrationRepository,_courseRepository,_studentRepository, new Student())
                    {
                        Student = parameter as Student
                    } as T,
                _ => throw new ArgumentException($"Unknown viewmodel type: {typeof(T)}")
            };
        }
    }
}