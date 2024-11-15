using System.Collections.ObjectModel;
using System.Windows;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.ViewModels;
using CoursesManager.MVVM.Messages;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Dialogs.Windows;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.AddressRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Views.Students;
using CoursesManager.UI.ViewModels.Courses;

namespace CoursesManager.UI;

public partial class App : Application
{
    //This is a temporary static class that will hold all the data that is used in the application.
    //This is a temporary solution until we have a database.
    public static ObservableCollection<Student> Students { get; private set; }

    public static ObservableCollection<Course> Courses { get; private set; }
    public static ObservableCollection<Location> Locations { get; private set; }
    public static ObservableCollection<Registration> Registrations { get; private set; }

    public static ICourseRepository CourseRepository { get; private set; } = new CourseRepository();
    public static ILocationRepository LocationRepository { get; private set; } = new LocationRepository();
    public static IRegistrationRepository RegistrationRepository { get; private set; } = new RegistrationRepository();
    public static IStudentRepository StudentRepository { get; private set; } = new StudentRepository();
    public static IAddressRepository AddressRepository { get; private set; } = new AddressRepository();

    public static INavigationService NavigationService { get; set; } = new NavigationService();
    public static IMessageBroker MessageBroker { get; set; } = new MessageBroker();
    public static IDialogService DialogService { get; set; } = new DialogService();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SetupDummyDataTemporary();

        RegisterViewModels();
        RegisterDialogs();

        MessageBroker.Subscribe<ApplicationCloseRequestedMessage, App>(ApplicationCloseRequestedHandler, this);

        NavigationService.NavigateTo<StudentManagerViewModel>();

        MainWindow mw = new()
        {
            DataContext = new MainWindowViewModel(NavigationService, MessageBroker)
        };
        mw.Show();
    }

    private static void SetupDummyDataTemporary()
    {
        //This is a temporary static class that will hold all the data that is used in the application.
        //This is a temporary solution until we have a database.
        Students = DummyDataGenerator.GenerateStudents(50);
        Courses = DummyDataGenerator.GenerateCourses(30);
        Locations = DummyDataGenerator.GenerateLocations(15);
        Registrations = DummyDataGenerator.GenerateRegistrations(50, 41);

        foreach (var registration in Registrations)
        {
            registration.Student = Students.FirstOrDefault(s => s.Id == registration.StudentID);
            registration.Course = Courses.FirstOrDefault(c => c.ID == registration.CourseID);
        }

        foreach (var course in Courses)
        {
            course.Location = Locations.FirstOrDefault(s => s.Id == course.LocationId);
        }
    }

    private void RegisterDialogs()
    {
        DialogService.RegisterDialog<YesNoDialogViewModel, YesNoDialogWindow, YesNoDialogResultType>((initial) => new YesNoDialogViewModel(initial));
        DialogService.RegisterDialog<ConfirmationDialogViewModel, ConfirmationDialogWindow, ConfirmationDialogResultType>((initial) => new ConfirmationDialogViewModel(initial));
        DialogService.RegisterDialog<AddStudentViewModel, AddStudentPopup, bool>((initial) => new AddStudentViewModel(
            initial,
            StudentRepository,
            CourseRepository,
            RegistrationRepository,
            DialogService
        ));
    }

    private void RegisterViewModels()
    {
        INavigationService.RegisterViewModelFactory(() => new StudentManagerViewModel(DialogService));
        INavigationService.RegisterViewModelFactory((nav) => new CoursesManagerViewModel(
            CourseRepository,
            RegistrationRepository,
            nav));
        INavigationService.RegisterViewModelFactory(() => new CourseOverViewViewModel());
        DialogService.RegisterDialog<EditStudentViewModel, EditStudentPopup, Student>(
        (student) => new EditStudentViewModel(
            StudentRepository,
            CourseRepository,
            RegistrationRepository,
            DialogService,
            student)
        );
    }

    /// <summary>
    /// Enables us to close the app by sending a message through the messenger.
    /// </summary>
    /// <param name="obj"></param>
    private static async void ApplicationCloseRequestedHandler(ApplicationCloseRequestedMessage obj)
    {
        var result = await DialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(new YesNoDialogResultType
        {
            DialogTitle = "CoursesManager",
            DialogText = "Wil je de app afsluiten?"
        });

        if (result.Data is null) return;

        if (result.Data.Result) Application.Current.Shutdown();
    }
}