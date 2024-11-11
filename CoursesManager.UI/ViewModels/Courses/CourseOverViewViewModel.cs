using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models.Repositories.CourseRepository;

namespace CoursesManager.UI.ViewModels.Courses
{
    internal class CourseOverViewViewModel : NavigatableViewModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;

        public ICommand ChangeCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        public Course CurrentCourse { get; set; }

        public CourseOverViewViewModel(ICourseRepository courseRepository, IDialogService dialogService, IMessageBroker messageBroker, INavigationService navigationService) : base(navigationService)
        {
            _courseRepository = courseRepository;
            _dialogService = dialogService;
            _messageBroker = messageBroker;

            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");

            DeleteCourseCommand = new RelayCommand(OnDelete);
        }

        private async void OnDelete()
        {
            if (_courseRepository.HasActiveRegistrations(CurrentCourse))
            {
                var result = await _dialogService.ShowDialogAsync<ErrorDialogViewModel, ConfirmationDialogResultType>(new ConfirmationDialogResultType
                {
                    DialogText = "Cursus heeft nog actieve registraties.",
                    DialogTitle = "Error"
                });
            }
            else
            {
                var result = await _dialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(new YesNoDialogResultType
                {
                    DialogTitle = "Bevestiging",
                    DialogText = "Weet je zeker dat je deze cursus wilt verwijderen?"
                });

                if (result.Outcome == DialogOutcome.Success && result.Data is not null && result.Data.Result)
                {
                    try
                    {
                        _courseRepository.Delete(CurrentCourse.ID);

                        await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(
                            new ConfirmationDialogResultType
                            {
                                DialogText = "Succesvol verwijderd",
                                DialogTitle = "Info"
                            });

                        _messageBroker.Publish(new CoursesChangedMessage());
                        _navigationService.GoBackAndClearForward();
                    }
                    catch (Exception ex)
                    {
                        //TODO: add logging
                        await _dialogService.ShowDialogAsync<ErrorDialogViewModel, ConfirmationDialogResultType>(new ConfirmationDialogResultType
                        {
                            DialogText = "Er is iets fout gegaan.",
                            DialogTitle = "Error"
                        });
                    }
                }
            }
        }

        private void ChangeCourse()
        {
        }
    }
}