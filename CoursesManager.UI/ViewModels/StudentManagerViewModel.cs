using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Models;
using CoursesManager.UI.ViewModels.Design;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : NavigatableViewModel
    {
        private CancellationTokenSource _cancellationTokenSource;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                // Debounce the filter operation
                DebounceFilterStudentRecords();
            }
        }


        private ObservableCollection<Student> _studentRecords;
        private ObservableCollection<Student> _filteredStudentRecords;
        public ObservableCollection<Student> FilteredStudentRecords
        {
            get => _filteredStudentRecords;
            set => SetProperty(ref _filteredStudentRecords, value);
        }

        public ICommand DataImportCommand { get; private set; }
        public ICommand DataExportCommand { get; private set; }
        public ICommand OpenRecordCommand { get; private set; }
        public ICommand DeleteRecordCommand { get; private set; }
        public ICommand EditRecordCommand { get; private set; }
        public ICommand AddRecordCommand { get; private set; }

        public StudentManagerViewModel(INavigationService navigationService) : base(navigationService)
        {
            ViewTitle = "Cursisten beheer";

            _studentRecords = DesignStudentManagerViewModel.GenerateRandomStudents(1200);
            FilteredStudentRecords = new ObservableCollection<Student>(_studentRecords);
        }

        private async void DebounceFilterStudentRecords()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            try
            {
                await Task.Delay(300, token); 

                await FilterStudentRecordsAsync(token);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        private async Task FilterStudentRecordsAsync(CancellationToken cancellationToken)
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
                }, cancellationToken);

                FilteredStudentRecords = new ObservableCollection<Student>(filtered);
            }


            OnPropertyChanged(nameof(FilteredStudentRecords));
        }
    }
}
