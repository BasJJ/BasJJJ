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
using CoursesManager.UI.Models.Repositories;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Views.Students;
using CoursesManager.UI.ViewModels.Courses;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesManager.UI;

public partial class App : Application
{
    //This is a temporary static class that will hold all the data that is used in the application.
    //This is a temporary solution until we have a database.
    public static ObservableCollection<Student> Students { get; private set; }
    public static ObservableCollection<Course> Courses { get; private set; }
    public static ObservableCollection<Location> Locations { get; private set; }
    public static ObservableCollection<Registration> Registrations { get; private set; }

    public static INavigationService NavigationService { get; set; } = new NavigationService();
    public static IMessageBroker MessageBroker { get; set; } = new MessageBroker();
    public static IDialogService DialogService { get; set; } = new DialogService();
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        
        //This is a temporary static class that will hold all the data that is used in the application.
        //This is a temporary solution until we have a database.
        Students = DummyDataGenerator.GenerateStudents(15);
        Courses = DummyDataGenerator.GenerateCourses(30);
        Locations = DummyDataGenerator.GenerateLocations(15);
        Registrations = DummyDataGenerator.GenerateRegistrations(15, 15);

        foreach (var registration in Registrations)
        {
            registration.Student = Students.FirstOrDefault(s => s.Id == registration.StudentID);
            registration.Course = Courses.FirstOrDefault(c => c.ID == registration.CourseID);
        }

        foreach (var course in Courses)
        {
            course.Location = Locations.FirstOrDefault(s => s.Id == course.LocationId);
        }

        RegisterViewModels();
        RegisterDialogs();

        // This implementation of the service collection we will use it until we make sure that it's needed for
        // our project according to the decision of The teacher.
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        MessageBroker.Subscribe<ApplicationCloseRequestedMessage, App>(ApplicationCloseRequestedHandler, this);

        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<CoursesManagerViewModel>();
 

        MainWindow mw = new()
        {
            DataContext = new MainWindowViewModel(NavigationService, MessageBroker)
        };
        mw.Show();
    }

    private void RegisterDialogs()
    {
        DialogService.RegisterDialog<YesNoDialogViewModel, YesNoDialogWindow, YesNoDialogResultType>((initial) => new YesNoDialogViewModel(initial));
        DialogService.RegisterDialog<ConfirmationDialogViewModel, ConfirmationDialogWindow, ConfirmationDialogResultType>((initial) => new ConfirmationDialogViewModel(initial));
        DialogService.RegisterDialog<AddStudentViewModel, AddStudentPopup, bool>((initial) => new AddStudentViewModel(
            initial,
            studentRepository: ServiceProvider.GetRequiredService<IStudentRepository>(), 
            courseRepository: ServiceProvider.GetRequiredService<ICourseRepository>(),
            registrationRepository: ServiceProvider.GetRequiredService<IRegistrationRepository>(),
            DialogService
        ));
    }

    // This method is used to register all services that are used in the application.
    // This way we can use dependency injection to inject the services where needed.
    // This method we will use it until we make sure that it's needed for our project according to the decision of 
    // The teacher.
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddSingleton<ILocationRepository, LocationRepository>();
        services.AddSingleton<IRegistrationRepository, RegistrationRepository>();
        services.AddSingleton<ICourseRepository, CourseRepository>();

        // Register view models and views
        services.AddTransient<MainWindow>();
        services.AddTransient<StudentManagerViewModel>();
        services.AddTransient<AddStudentViewModel>();
        services.AddTransient<CoursesManagerViewModel>();
        // Register other view models...
    }
   

    private void RegisterViewModels()
    {
        INavigationService.RegisterViewModelFactory(() => new StudentManagerViewModel(DialogService));
        INavigationService.RegisterViewModelFactory((nav) => new CoursesManagerViewModel(ServiceProvider.GetService<ICourseRepository>(), ServiceProvider.GetService<IRegistrationRepository>(), nav));
        INavigationService.RegisterViewModelFactory(() => new CourseOverViewViewModel());

        INavigationService.RegisterViewModelFactory(() => new TestViewModel());
        DialogService.RegisterDialog<EditStudentViewModel, EditStudentPopup, Student>(
        (student) => new EditStudentViewModel(
            ServiceProvider.GetRequiredService<IStudentRepository>(),
            ServiceProvider.GetRequiredService<ICourseRepository>(),
            ServiceProvider.GetRequiredService<IRegistrationRepository>(),
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