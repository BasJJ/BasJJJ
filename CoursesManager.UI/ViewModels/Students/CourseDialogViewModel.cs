using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoursesManager.UI.Dialogs.Enums;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Dialogs.ResultTypes;





namespace CoursesManager.UI.ViewModels.Students
{
    class CourseDialogViewModel : DialogViewModel<Course>, INotifyPropertyChanged
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
            get => IsDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }
        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public event EventHandler<Course> CourseAdded;

        public Course Course
        {
            get => _course;
            set
            {
                _course = value;
                OnPropertyChanged(nameof(Course));
            }
        }

        public ICommand SaveCommand {  get;  }
        public ICommand CancelCommandd { get; }
        

        public ICommand UploadCommand { get; }
        public ObservableCollection<string> Courses
        {
            get; set;
        }

        public ObservableCollection<Location> Locations { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        //public Window ParentWindow { get; set; }

        public CourseDialogViewModel(ICourseRepository courseRepository,  IDialogService dialogService, ILocationRepository locationRepository, Course? course) : base(course)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));

            Course = course ?? new Course 
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
            CancelCommandd = new RelayCommand( OnCancel);
            UploadCommand = new RelayCommand(UploadImage);
            PropertyChanged = delegate { };

        }



        public CourseDialogViewModel(Course? dialogResultType) : base(dialogResultType)
        {
        }

        protected override void InvokeResponseCallback(DialogResult<Course> dialogResult)
        {
            ResponseCallback.Invoke(dialogResult);
        }
        private async Task OnSaveAsync()
        {

            if (!FieldsValidations())
            {
                var dialogResult = DialogResult<bool>.Builder()
                    .SetSuccess(false, "Vereiste velden moeten correct worden ingevuld")
                    .Build();
                ShowWarningDialog(dialogResult);
                return;
            }

            // Controleer of de cursus uniek is
            if (_courseRepository.GetAll().Any(c => c.Name.Equals(Course.Name, StringComparison.OrdinalIgnoreCase)))
            {
                var dialogResult = DialogResult<bool>.Builder()
                    .SetSuccess(false, "De cursusnaam bestaat al")
                    .Build();
                ShowWarningDialog(dialogResult);
                return;
            }

            // Voeg de cursus toe
            _courseRepository.Add(Course);

            var successDialogResult = DialogResult<Course>.Builder()
                .SetSuccess(Course, "Cursus succesvol toegevoegd")
                .Build();



            ShowSuccessDialog(successDialogResult.OutcomeMessage);

            //await ShowSuccessDialog(DialogType.Notify, "Cursus Succesvol opgeslagen.");

            InvokeResponseCallback(successDialogResult);
        }


        private bool FieldsValidations()
        {
            if (string.IsNullOrEmpty(Course.Name?.Trim()))
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De cursusnaam is verplicht.")
                    .Build());
                return false;
            }

            if (string.IsNullOrEmpty(Course.Code?.Trim()))
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De cursuscode is verplicht.")
                    .Build());
                return false;
            }

            if (Course.EndDate == DateTime.Now)
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De einddatum is verplicht.")
                    .Build());
                return false;
            }

            if (Course.StartDate == DateTime.Now)
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De begindatum is verplicht.")
                    .Build());
                return false;
            }

            if (Course.Location == null)
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De locatie is verplicht.")
                    .Build());
                return false;
            }

            if (string.IsNullOrEmpty(Course.Description?.Trim()))
            {
                ShowWarningDialog(DialogResult<bool>.Builder()
                    .SetSuccess(false, "De beschrijving is verplicht.")
                    .Build());
                return false;
            }


            return true;
        }


        private async Task<bool> ShowDialogAsync(DialogType dialogType, string message, string dialogTitle) 
        { 
            void SetIsDialogOpen(bool value) 
            { 
                if (Application.Current?.Dispatcher?.CheckAccess() == true) 
                { 
                    IsDialogOpen = value; 
                } 
                else 
                { 
                    Application.Current?.Dispatcher?.Invoke(() => IsDialogOpen = value); 
                } 
            } 
            switch (dialogType) 
            { 
                case DialogType.Notify: 
                    SetIsDialogOpen(true); 
                    await _dialogService.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(
                        new DialogResultType
                        { 
                            DialogTitle = dialogTitle, 
                            DialogText = message 
                        }); 
                    SetIsDialogOpen(false); 
                    return true; 
                case DialogType.Confirmation: 
                    SetIsDialogOpen(true);
                    var result = await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(
                         new ConfirmationDialogResultType
                         {
                             DialogTitle = dialogTitle,
                             DialogText = message
                         });

                    SetIsDialogOpen(false); 
                    return result?.Data?.Result ?? false; 
                default: 
                    throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null); 
            } 
        }

        protected virtual void ShowSuccessDialog(string succesMessage)
        {
            MessageBox.Show(succesMessage, "Succes melding", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected virtual void ShowWarningDialog(DialogResult<bool> dialogResult)
        {
            MessageBox.Show(dialogResult.OutcomeMessage, "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void OnCancel()
        {
            var dialogResult = DialogResult<Course>.Builder()
                .SetCanceled("Changes were canceled by the user.")
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
