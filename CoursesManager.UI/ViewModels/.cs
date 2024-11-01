using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Printing;
using System.Windows;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Views.Students;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : ViewModel
    {
        #region View fields

        public ObservableCollection<Student> students;
        private string _searchText;
        private readonly IDialogService _dialogService;
        private StudentRepository _studentRepository;

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

        #endregion View fields

        public StudentManagerViewModel(IDialogService dialogService)
        {
            ViewTitle = "Cursisten beheer";

            _dialogService = dialogService;

            _studentRepository = new StudentRepository();
            LoadStudents();
            AddStudentCommand = new RelayCommand(OpenAddStudentPopup);
            SearchCommand = new RelayCommand(OnSearchCommand);
        }

        private void LoadStudents()
        {
            students = new ObservableCollection<Student>(_studentRepository.GetAll());
            FilteredStudentRecords = new ObservableCollection<Student>(students);
        }

        #region Commands

        public ICommand DataImportCommand { get; private set; }
        public ICommand DataExportCommand { get; private set; }
        public ICommand OpenRecordCommand { get; private set; }
        public ICommand DeleteRecordCommand { get; private set; }
        public ICommand EditRecordCommand { get; private set; }
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
                FilteredStudentRecords = new ObservableCollection<Student>(students);
            }
            else
            {
                string searchTerm = SearchText.Trim().Replace(" ", "").ToLower();

                var filtered = await Task.Run(() =>
                {
                    return students.Where(student => student.TableFilter().ToLower().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                });

                FilteredStudentRecords = new ObservableCollection<Student>(filtered);
            }

            OnPropertyChanged(nameof(FilteredStudentRecords));
        }

        private async void OpenAddStudentPopup()
        {
            var dialogResult = await _dialogService.ShowDialogAsync<AddStudentViewModel, bool>(true);
            System.Diagnostics.Debug.WriteLine($"DialogResult created with message: {dialogResult.OutcomeMessage}");

            if (dialogResult != null && dialogResult.Data != null && dialogResult.Outcome == DialogOutcome.Success)
            {
                MessageBox.Show(dialogResult.OutcomeMessage, "Succes melding", MessageBoxButton.OK, MessageBoxImage.Information);
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
