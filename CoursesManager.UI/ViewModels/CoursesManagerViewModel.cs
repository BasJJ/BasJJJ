using System.Collections.ObjectModel;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;

namespace CoursesManager.UI.ViewModels
{
    public class CoursesManagerViewModel : ViewModel
    {
        // Properties
        private readonly ICourseRepository _courseRepository;
        private string _searchText = String.Empty;
        private bool _isToggled = true;

        // Getters and Setters
        public ICommand SearchCommand { get; }
        public ICommand ToggleCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand CourseOptionCommand { get; }

        public ObservableCollection<Course> Courses { get; }
        public ObservableCollection<Course> FilteredCourses { get; }

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

        // Contructor
        public CoursesManagerViewModel(ICourseRepository CourseRepository, IRegistrationRepository registrationRepository)
        {
            ViewTitle = "Cursus beheer";

            SearchCommand = new RelayCommand(() => _ = FilterRecordsAsync());
            ToggleCommand = new RelayCommand(() => _ = FilterRecordsAsync());
            
            Courses = new ObservableCollection<Course>(CourseRepository.GetAll());
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
    }
}