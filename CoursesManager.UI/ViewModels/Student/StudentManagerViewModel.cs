using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Printing;
using System.Windows;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Views.Students;
using CoursesManager.UI.Utils;
using CoursesManager.UI.Models.CoursesManager.UI.Models;
using System;


namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : ViewModel
    {
        #region View fields

        public ObservableCollection<Student> students;
        private string _searchText;
        private readonly IDialogService _dialogService;
        private readonly StudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IRegistrationRepository _registrationRepository;

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));

                    // When the student changes, update the courses
                    UpdateStudentCourses();
                }
            }
        }


        private ObservableCollection<CourseStudentPayment> _coursePaymentList;
        public ObservableCollection<CourseStudentPayment> CoursePaymentList
        {
            get => _coursePaymentList;
            set => SetProperty(ref _coursePaymentList, value);
        }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private ObservableCollection<Student> _filteredStudentRecords;

        public ObservableCollection<Student> FilteredStudentRecords
        {
            get => _filteredStudentRecords;
            set => SetProperty(ref _filteredStudentRecords, value);
        }

        public ObservableCollection<Course> Courses { get; set; }

        #endregion View fields

        public StudentManagerViewModel(IDialogService dialogService)
        {
            ViewTitle = "Cursisten beheer";

            _dialogService = dialogService;

            _studentRepository = new StudentRepository();
            LoadStudents();
            AddStudentCommand = new RelayCommand(OpenAddStudentPopup);
            EditStudentCommand = new RelayCommand<Student>(OpenEditStudentPopup, (s) => true);
            DeleteStudentCommand = new RelayCommand<Student>(OpenDeleteStudentPopup, (s) => s != null);
            SearchCommand = new RelayCommand(OnSearchCommand);

            if (SelectedStudent != null && SelectedStudent.Courses != null)
            {
                Courses = SelectedStudent.Courses;
                ObservableCollection<Registration> registration = DummyDataGenerator.GenerateRegistrations(Courses.Count, 1);
                CoursePaymentList = new ObservableCollection<CourseStudentPayment>();

                for (int i = 0; i < Courses.Count - 1; i++)
                {
                    CourseStudentPayment coursePayment = new CourseStudentPayment(Courses[i], registration[i]);
                    CoursePaymentList.Add(coursePayment);
                }

            }



        }

        public ObservableCollection<CourseStudentPayment> DisplayedCourses { get; set; }

        private void UpdateStudentCourses()
        {
            if (SelectedStudent != null && SelectedStudent.Courses != null)
            {
                Courses = SelectedStudent.Courses;
                ObservableCollection<Registration> registration = DummyDataGenerator.GenerateRegistrations(Courses.Count, 1);
                CoursePaymentList = new ObservableCollection<CourseStudentPayment>();

                for (int i = 0; i < Courses.Count - 1; i++)
                {
                    CourseStudentPayment coursePayment = new CourseStudentPayment(Courses[i], registration[i]);
                    CoursePaymentList.Add(coursePayment);
                }

                DisplayedCourses = new ObservableCollection<CourseStudentPayment>(CoursePaymentList);

            }else
            {
                DisplayedCourses = new ObservableCollection<CourseStudentPayment>();
            }
            OnPropertyChanged(nameof(DisplayedCourses));
        }


        private void LoadStudents()
        {
            students = new ObservableCollection<Student>(_studentRepository.GetAll().Where(s => !s.Is_deleted));
            FilteredStudentRecords = new ObservableCollection<Student>(students);
        }

        #region Commands

        public ICommand DataImportCommand { get; private set; }
        public ICommand DataExportCommand { get; private set; }
        public ICommand OpenRecordCommand { get; private set; }
        public ICommand DeleteStudentCommand { get; private set; }
        public ICommand EditStudentCommand { get; private set; }
        public ICommand AddStudentCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        #region SearchCommand

        private void OnSearchCommand()
        {
            FilterStudentRecordsAsync();
        }

        #endregion SearchCommand

        #endregion Commands

        private async Task FilterStudentRecordsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Filter to include only active students
                FilteredStudentRecords = new ObservableCollection<Student>(students.Where(s => !s.Is_deleted));
            }
            else
            {
                string searchTerm = SearchText.Trim().Replace(" ", "").ToLower();

                var filtered = await Task.Run(() =>
                {
                    return students
                        .Where(s => !s.Is_deleted) // Only include non-deleted students
                        .Where(student => student.TableFilter().ToLower().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                });

                FilteredStudentRecords = new ObservableCollection<Student>(filtered);
            }

            OnPropertyChanged(nameof(FilteredStudentRecords));
        }


        private async void OpenAddStudentPopup()
        {
            IsDialogOpen = true;
            var dialogResult = await _dialogService.ShowDialogAsync<AddStudentViewModel, bool>(true);

            if (dialogResult != null && dialogResult.Data != null && dialogResult.Outcome == DialogOutcome.Success)
            {
                LoadStudents();
            }
            IsDialogOpen = false;
        }


        private async void OpenEditStudentPopup(Student student)
        {
            if (student == null) return;
            IsDialogOpen = true;
            var dialogResult = await _dialogService.ShowDialogAsync<EditStudentViewModel, Student>(student);

            if (dialogResult != null && dialogResult.Data != null && dialogResult.Outcome == DialogOutcome.Success)
            {
                LoadStudents();
            }
            IsDialogOpen = false;
        }


        private async void OpenDeleteStudentPopup(Student student)
        {
            if (student == null) return;
            //temp
            IsDialogOpen = true;
            var result = await _dialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(
                new YesNoDialogResultType
                {
                    DialogTitle = "Bevestiging",
                    DialogText = "Wilt u deze cursist verwijderen?"
                });
            IsDialogOpen = false;

            if (result?.Data?.Result == true)
            {
                student.Is_deleted = true;
                student.date_deleted = DateTime.Now;
                _studentRepository.Update(student);

                await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(
                    new ConfirmationDialogResultType
                    {
                        DialogTitle = "Informatie",
                        DialogText = "Cursist succesvol verwijderd."
                    });
                //temp

                LoadStudents();
            }
        }


        private void OnStudentAdded(object sender, Student e)
        {
            LoadStudents();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




