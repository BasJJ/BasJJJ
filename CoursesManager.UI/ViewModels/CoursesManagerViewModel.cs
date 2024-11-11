using System.Collections.ObjectModel;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.ViewModels.Courses;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;

namespace CoursesManager.UI.ViewModels
{
    public class CoursesManagerViewModel : NavigatableViewModel
    {
        // Properties
        private readonly ICourseRepository _courseRepository;

        private string _searchText = String.Empty;
        private bool _isToggled = true;
        private readonly IMessageBroker _messageBroker;

        // Getters and Setters
        public ICommand SearchCommand { get; }

        public ICommand ToggleCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand CourseOptionCommand { get; }

        public ObservableCollection<Course> Courses { get; private set; }
        public ObservableCollection<Course> FilteredCourses { get; private set; }

        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) _ = FilterRecordsAsync(); }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set { if (SetProperty(ref _isToggled, value)) _ = FilterRecordsAsync(); }
        }

        // Constructor
        public CoursesManagerViewModel(ICourseRepository courseRepository, IMessageBroker messageBroker, INavigationService navigationService) : base(navigationService)
        {
            _courseRepository = courseRepository;
            _messageBroker = messageBroker;
            _messageBroker.Subscribe<CoursesChangedMessage, CoursesManagerViewModel>(OnCoursesChangedMessage, this);

            ViewTitle = "Cursus beheer";

            SearchCommand = new RelayCommand(() => _ = FilterRecordsAsync());
            ToggleCommand = new RelayCommand(() => _ = FilterRecordsAsync());
            CourseOptionCommand = new RelayCommand<Course>(OpenCourseOptions);

            LoadCourses();
        }

        private void OnCoursesChangedMessage(CoursesChangedMessage obj)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            Courses = new ObservableCollection<Course>(_courseRepository.GetAll());
            FilteredCourses = new ObservableCollection<Course>(Courses);

            FilterRecordsAsync();
        }

        // Methods
        private void OnSearchCommand() => FilterRecordsAsync();

        private void OnToggleCommand() => FilterRecordsAsync();

        private async Task FilterRecordsAsync()
        {
            string searchTerm = string.IsNullOrWhiteSpace(SearchText)
                ? String.Empty
                : SearchText.Trim().Replace(" ", "").ToLower();

            var filtered = await Task.Run(() =>
                Courses.Where(course =>
                    (string.IsNullOrEmpty(searchTerm)
                    || course.GenerateFilterString().ToLower().Contains(searchTerm))
                    && course.IsActive == IsToggled
                ).OrderBy(course => course.IsPayed)
                .ThenBy(course => course.StartDate)
                .ToList());

            UpdateFilteredCourses(filtered);
        }

        private void UpdateFilteredCourses(IEnumerable<Course> filteredCourses)
        {
            FilteredCourses.Clear();
            foreach (var course in filteredCourses) FilteredCourses.Add(course);
        }

        private void OpenCourseOptions(Course parameter)
        {
            GlobalCache.Instance.Put("Opened Course", parameter, false);
            _navigationService.NavigateTo<CourseOverViewViewModel>();
        }
    }
}