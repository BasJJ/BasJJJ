using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using System.Windows.Controls;

namespace CoursesManager.UI.ViewModels.Students
{
    class CourseDialogViewModel : DialogViewModel<Course>
    {

        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly ILocationRepository _locationRepository;
        private BitmapImage? _imageSource;
        private Course? _course;

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }

        public BitmapImage? ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public Course? Course
        {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            private set => SetProperty(ref _canSave, value);
        }


        public ICommand SaveCommand { get; }


        public ICommand CancelCommand { get; }


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

            OriginalCourse = course;
            Locations = new ObservableCollection<Location>(locationRepository.GetAll());

            Courses = new ObservableCollection<string>(_courseRepository.GetAll().Select(c => c.Name));
            SaveCommand = new RelayCommand(ExecuteSave, () => CanSave);
            CancelCommand = new RelayCommand(OnCancel);
            UploadCommand = new RelayCommand(UploadImage);

            Course.PropertyChanged += (_, _) => UpdateCanSave();
            UpdateCanSave();
        }

        private Course? OriginalCourse { get; }
        protected override void InvokeResponseCallback(DialogResult<Course> dialogResult)
        {
            ResponseCallback.Invoke(dialogResult);
        }

        private void NotifyCanExecuteChanged()
        {
            (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private async void ExecuteSave()
        {
            try
            {
                await OnSaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateCanSave()
        {
            var fieldsAreValid = !string.IsNullOrWhiteSpace(Course!.Name)
                                 && !string.IsNullOrWhiteSpace(Course.Code)
                                 && Course.StartDate != default
                                 && Course.EndDate != default
                                 && Course.Location != null
                                 && !string.IsNullOrWhiteSpace(Course.Description);

            Console.WriteLine($"Fields Valid: {fieldsAreValid}");

            var noValidationErrors = !HasValidationErrors();

            Console.WriteLine($"No Validation Errors: {noValidationErrors}");

            CanSave = fieldsAreValid && noValidationErrors;

            NotifyCanExecuteChanged();
        }


        private bool HasValidationErrors()
        {
            if (Application.Current.MainWindow == null)
            {
                return false;
            }

            foreach (var child in LogicalTreeHelper.GetChildren(Application.Current.MainWindow))
            {
                if (child is DependencyObject dependencyObject && Validation.GetHasError(dependencyObject))
                {

                    if (dependencyObject is Image && Course!.Image == null)
                        continue;

                    return true;
                }
            }
            return false;
        }


        private async Task OnSaveAsync()
        {

            if (OriginalCourse == null)
            {
                _courseRepository.Add(Course!);
                await ShowSuccessDialog("Cursus succesvol toegevoegd.");
            }
            else
            {
                _courseRepository.Update(Course!);
                await ShowSuccessDialog("Cursus succesvol bijgewerkt.");
            }

            var successDialogResult = DialogResult<Course>.Builder()
                .SetSuccess(Course!, OriginalCourse == null ? "Cursus succesvol toegevoegd" : "Cursus succesvol bijgewerkt")
                .Build();

            InvokeResponseCallback(successDialogResult);
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
                Course!.Image = new BitmapImage(new Uri(openDialog.FileName));
            }
        }
    }
}
