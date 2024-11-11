//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using CoursesManager.UI.Models;

//namespace CoursesManager.MVVM.Utils;
 

//public class DataUtil
//{
//    private readonly Timer _refreshTimer;
//    public ObservableCollection<Student> Students { get; private set; }
//    public ObservableCollection<Course> Courses { get; private set; }
//    private readonly IDatabaseService _databaseService;

//    public DataUtil(IDatabaseService databaseService, double refreshIntervalInMinutes = 5)
//    {
//        _databaseService = databaseService;

//        // Set up the timer for refreshing data at the specified interval
//        _refreshTimer = new Timer(refreshIntervalInMinutes * 60 * 1000);
//        _refreshTimer.Elapsed += async (sender, args) => await RefreshData();
//    }

//    public async Task LoadInitialData()
//    {
//        await LoadStudents();
//        await LoadCourses();
//    }

//    public async Task LoadStudents()
//    {
//        // Fetch students from the database
//        var students = await _databaseService.GetStudentsAsync();
//        Students = new ObservableCollection<Student>(students);
//    }

//    public async Task LoadCourses()
//    {
//        // Fetch courses from the database
//        var courses = await _databaseService.GetCoursesAsync();
//        Courses = new ObservableCollection<Course>(courses);
//    }

//    public void StartAutoRefresh()
//    {
//        _refreshTimer.Start();
//    }

//    public void StopAutoRefresh()
//    {
//        _refreshTimer.Stop();
//    }

//    private async Task RefreshData()
//    {
//        await LoadStudents();
//        await LoadCourses();
//    }
//}
