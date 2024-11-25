using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using System.Windows.Controls;

namespace CoursesManager.UI.ViewModels.Students
{
    class CourseDialogViewModel : DialogViewModel<Course>
    {

        private readonly ICourseRepository _courseRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IDialogService _dialogService;
        private readonly ILocationRepository _locationRepository;
        private BitmapImage _imageSource;
        private Course _course;

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public event EventHandler<Course> CourseAdded;

        public Course Course
        {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommandd { get; }


        public ICommand UploadCommand { get; }
        public ObservableCollection<string> Courses
        {
            get; set;
        }

        public ObservableCollection<Location> Locations { get; set; }

        public CourseDialogViewModel(ICourseRepository courseRepository, IDialogService dialogService, ILocationRepository locationRepository, Course? course) : base(course)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));

            Course = course != null
                   ? course.Copy()
                    : new Course
    {
                Name = string.Empty,
                Code = string.Empty,
                Description = string.Empty,
                Location = null,
                IsActive = false,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Image = null,

            };

            Locations = new ObservableCollection<Location>(locationRepository.GetAll());

            Courses = new ObservableCollection<string>(_courseRepository.GetAll().Select(c => c.Name));
            SaveCommand = new RelayCommand(async () => await OnSaveAsync());
            CancelCommandd = new RelayCommand(OnCancel);
            UploadCommand = new RelayCommand(UploadImage);
        }

        protected override void InvokeResponseCallback(DialogResult<Course> dialogResult)
        {
            ResponseCallback.Invoke(dialogResult);
        }

        private bool AreFieldsValid()
        {
            var isValid = !Validation.GetHasError(Application.Current.MainWindow);
            foreach (var child in LogicalTreeHelper.GetChildren(Application.Current.MainWindow))
            {
                if (child is DependencyObject dependencyObject && Validation.GetHasError(dependencyObject))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        private async Task OnSaveAsync()
        {
            
            if (!AreFieldsValid())
            {
                
                return;
            }

            if (_courseRepository.GetAll().Any(c => c.Name.Equals(Course.Name, StringComparison.OrdinalIgnoreCase)))
            {
                
                await ShowWarningDialog("De cursusnaam bestaat al");
                return;
            }

            
            _courseRepository.Add(Course);

            
            await ShowSuccessDialog("Cursus succesvol toegevoegd");

            
            var successDialogResult = DialogResult<Course>.Builder()
                .SetSuccess(Course, "Cursus succesvol toegevoegd")
                .Build();

            InvokeResponseCallback(successDialogResult);
        }




        private bool FieldsValidations()
        {
            // Hier kun je specifieke validatieberichten toevoegen als dat nodig is
            if (string.IsNullOrEmpty(Course.Name?.Trim()))
                return false;

            if (string.IsNullOrEmpty(Course.Code?.Trim()))
                return false;

            if (Course.EndDate == DateTime.Now)
                return false;

            if (Course.StartDate == DateTime.Now)
                return false;

            if (Course.Location == null)
                return false;

            if (string.IsNullOrEmpty(Course.Description?.Trim()))
                return false;

            return true;
        }

        private async Task ShowSuccessDialog(string message)
        {
            IsDialogOpen = true;

            await _dialogService.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(
                new DialogResultType
                {
                    DialogTitle = "Succes",
                    DialogText = message
                });

            IsDialogOpen = false;
        }

        private async Task ShowWarningDialog(string message)
        {
            IsDialogOpen = true;

            await _dialogService.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(
                new DialogResultType
                {
                    DialogTitle = "Waarschuwing",
                    DialogText = message
                });

            IsDialogOpen = false;
        }

        public void OnCancel()
        {
            var dialogResult = DialogResult<Course>.Builder()
                .SetCanceled("Wijzigingen geannuleerd door de gebruiker.")
                .Build();

            InvokeResponseCallback(dialogResult);
        }

        private void UploadImage()
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.bmp;*.jpg;*.png",
                FilterIndex = 1
            };

            if (openDialog.ShowDialog() == true)
            {
                ImageSource = new BitmapImage(new Uri(openDialog.FileName));
            }
        }
    }
}
