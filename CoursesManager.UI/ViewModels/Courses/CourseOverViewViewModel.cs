﻿using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Mail.MailService;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Mailing;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models;
using CoursesManager.UI.Repositories.CourseRepository;
using CoursesManager.UI.Repositories.RegistrationRepository;
using CoursesManager.UI.Repositories.StudentRepository;
using CoursesManager.UI.ViewModels.Students;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace CoursesManager.UI.ViewModels.Courses
{
    public class CourseOverViewViewModel : ViewModelWithNavigation
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;
        private readonly IMailProvider _mailProvider;
        public ICommand ChangeCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }
        public ICommand CheckboxChangedCommand { get; }

        public ICommand CertificateMailCommand { get; set; }
        public ICommand PaymentMailCommand { get; set; }
        public ICommand StartCourseMailCommand { get; set; }

        private readonly IStudentRepository _studentRepository;
        private readonly IRegistrationRepository _registrationRepository;

        private Course _currentCourse;

        public Course CurrentCourse
        {
            get => _currentCourse;
            private set => SetProperty(ref _currentCourse, value);
        }

        private bool _isPaid;
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }

        private bool _hasStarted;
        public bool HasStarted
        {
            get => _hasStarted;
            set => SetProperty(ref _hasStarted, value);
        }

        private bool _isFinished;
        public bool IsFinished
        {
            get => _isFinished;
            set => SetProperty(ref _isFinished, value);
        }

        private ObservableCollection<Student> _students;

        private ObservableCollection<CourseStudentPayment> _studentPayments;

        public ObservableCollection<CourseStudentPayment> StudentPayments
        {
            get => _studentPayments;
            private set => SetProperty(ref _studentPayments, value);
        }

        public CourseOverViewViewModel(IStudentRepository studentRepository, IRegistrationRepository registrationRepository, ICourseRepository courseRepository, IDialogService dialogService, IMessageBroker messageBroker, INavigationService navigationService) : base(navigationService)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));

            _courseRepository = courseRepository;
            _dialogService = dialogService;
            _messageBroker = messageBroker;
            _mailProvider = new MailProvider();

            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CheckboxChangedCommand = new RelayCommand<CourseStudentPayment>(OnCheckboxChanged);
            PaymentMailCommand = new RelayCommand(SendPaymentMail);
            StartCourseMailCommand = new RelayCommand(SendStartCourseMail);
            CertificateMailCommand = new RelayCommand(SendCertificateMail);


            LoadCourseData();

        }

        private void LoadCourseData()
        {
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");

            if (CurrentCourse == null)
            {
                throw new InvalidOperationException("No course is currently opened. Ensure the course is loaded in the GlobalCache.");
            }

            IsPaid = !CurrentCourse.IsPayed;
            HasStarted = false;
            IsFinished = false;
            if ((CurrentCourse.StartDate - DateTime.Now).TotalDays <= 7 && CurrentCourse.StartDate > DateTime.Now)
            {
                HasStarted = true;
            }
            if (CurrentCourse.EndDate <= DateTime.Now)
            {
                HasStarted = false;
                IsPaid = false;
                IsFinished = true;
            }

            var registrations = _registrationRepository.GetAllRegistrationsByCourse(CurrentCourse);

            var payments = registrations
                .Where(registration => registration.Student != null)
                .Select(registration => new CourseStudentPayment(registration.Student, registration))
                .ToList();

            StudentPayments = new ObservableCollection<CourseStudentPayment>(payments);
            OnPropertyChanged(nameof(StudentPayments));
        }

        private void OnCheckboxChanged(CourseStudentPayment payment)
        {
            if (payment == null || CurrentCourse == null) return;

            var currentRegistration = _registrationRepository.GetAllRegistrationsByCourse(CurrentCourse).FirstOrDefault(r => r.StudentId == payment.Student?.Id);

            if (currentRegistration != null)
            {
                try
                {
                    // Haal de true of false checkbox op voor paid, achieved en update de velden zodra deze gewijzigd worden.
                    currentRegistration.PaymentStatus = payment.IsPaid;
                    currentRegistration.IsAchieved = payment.IsAchieved;
                    _registrationRepository.Update(currentRegistration);

                    // Haal alle registrations van de CurrentCourse op en check of alle studenten betaald hebben zet dan CurrentCourse.IsPayed op true, zo niet dan false.
                    var allCurrentRegistrations = _registrationRepository.GetAllRegistrationsByCourse(CurrentCourse);
                    if (allCurrentRegistrations.All(r => r.PaymentStatus))
                    {
                        CurrentCourse.IsPayed = true;
                        _courseRepository.Update(CurrentCourse);
                    }
                    else
                    {
                        CurrentCourse.IsPayed = false;
                    }
                }
                catch
                (Exception ex)
                {
                    throw new Exception("No registration found");
                }
            }
            LoadCourseData();
        }

        private async void DeleteCourse()
        {

            await ExecuteWithOverlayAsync(_messageBroker, async () =>
            {
                if (_registrationRepository.GetAllRegistrationsByCourse(CurrentCourse).Any())
                {
                    var result = await _dialogService.ShowDialogAsync<ErrorDialogViewModel, DialogResultType>(
                        new DialogResultType
                        {
                            DialogText = "Cursus heeft nog actieve registraties.",
                            DialogTitle = "Error"
                        });
                }
                else
                {
                    var result = await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, DialogResultType>(
                        new DialogResultType
                        {
                            DialogTitle = "Bevestiging",
                            DialogText = "Weet je zeker dat je deze cursus wilt verwijderen?"
                        });
                    if (result.Outcome == DialogOutcome.Success && result.Data is not null && result.Data.Result)
                    {
                        try
                        {
                            _courseRepository.Delete(CurrentCourse);

                            _messageBroker.Publish(new CoursesChangedMessage());
                            _navigationService.GoBackAndClearForward();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(ex.Message);
                            await _dialogService.ShowDialogAsync<ErrorDialogViewModel, DialogResultType>(
                                new DialogResultType
                                {
                                    DialogText = "Er is iets fout gegaan.",
                                    DialogTitle = "Error"
                                });
                        }
                    }
                }
            });
        }

        public async void SendPaymentMail()
        {
            await _mailProvider.SendPaymentNotifications(CurrentCourse);
        }

        public async void SendStartCourseMail()
        {
           await _mailProvider.SendCourseStartNotifications(CurrentCourse);
        }

        public async void SendCertificateMail()
        {
            await _mailProvider.SendCertificates(CurrentCourse);
        }

        private async void ChangeCourse()
        {
            await ExecuteWithOverlayAsync(_messageBroker, async () =>
            {
                var dialogResult = await _dialogService.ShowDialogAsync<CourseDialogViewModel, Course>(CurrentCourse);
                

                if (dialogResult.Outcome == DialogOutcome.Success)
                {
                    _messageBroker.Publish(new CoursesChangedMessage());
                    CurrentCourse = dialogResult.Data;
                }

            });
        }
    }
}