using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Models;
using CoursesManager.UI.ViewModels.Design;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : ViewModel
    {
        #region View fields

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private ObservableCollection<Student> _studentRecords;
        private ObservableCollection<Student> _filteredStudentRecords;

        public ObservableCollection<Student> FilteredStudentRecords
        {
            get => _filteredStudentRecords;
            set => SetProperty(ref _filteredStudentRecords, value);
        }

        #endregion View fields

        public StudentManagerViewModel()
        {
            ViewTitle = "Cursisten beheer";

            _studentRecords = DesignStudentManagerViewModel.GenerateRandomStudents(150);
            FilteredStudentRecords = new ObservableCollection<Student>(_studentRecords);

            SearchCommand = new RelayCommand(OnSearchCommand);
        }

        #region Commands

        public ICommand DataImportCommand { get; private set; }
        public ICommand DataExportCommand { get; private set; }
        public ICommand OpenRecordCommand { get; private set; }
        public ICommand DeleteRecordCommand { get; private set; }
        public ICommand EditRecordCommand { get; private set; }
        public ICommand AddRecordCommand { get; private set; }

        #region SearchCommand

        public ICommand SearchCommand { get; private set; }

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
                FilteredStudentRecords = new ObservableCollection<Student>(_studentRecords);
            }
            else
            {
                string searchTerm = SearchText.Trim().ToLower();

                var filtered = await Task.Run(() =>
                {
                    return _studentRecords.Where(student => student.TableFilter().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                });

                FilteredStudentRecords = new ObservableCollection<Student>(filtered);
            }

            OnPropertyChanged(nameof(FilteredStudentRecords));
        }
    }
}