using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using static CoursesManager.UI.ViewModels.StudentManagerViewModel;
using System.Windows;

namespace CoursesManager.UI.ViewModels
{
    public class EditStudentViewModel : DialogViewModel<Student>, INotifyPropertyChanged
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;

        public event EventHandler CloseRequested;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Student Student { get; }

        public ObservableCollection<Course> AllCourses { get; }
        public ObservableCollection<Course> SelectedCourses { get; }

        public EditStudentViewModel(IStudentRepository studentRepository, ICourseRepository courseRepository, Student student)
        : base(student)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;

            Student = student ?? new Student();

            AllCourses = new ObservableCollection<Course>(_courseRepository.GetAll());

            if (Student.Courses == null)
            {
                Student.Courses = new ObservableCollection<Course>();
            }

            SelectedCourses = new ObservableCollection<Course>(Student.Courses);

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void ShowSuccessDialog(DialogResult<Student> dialogResult)
        {
            MessageBox.Show(dialogResult.OutcomeMessage, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnSave()
        {
            if (ValidateFields())
            {
                Student.Courses.Clear();
                foreach (var course in SelectedCourses)
                {
                    Student.Courses.Add(course);
                }

                _studentRepository.Update(Student);

                var result = DialogResult<Student>.Builder()
                    .SetSuccess(Student, "Student succesvol opgeslagen.")
                    .Build();

                ShowSuccessDialog(result);

                InvokeResponseCallback(result);
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowWarningDialog("Controleer of alle velden correct zijn ingevuld.");
            }
        }


        private void OnCancel()
        {
            var result = DialogResult<Student>.Builder()
                .SetCanceled("Wijziging is geannuleerd door de gebruiker.")
                .Build();

            InvokeResponseCallback(result);
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }


        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Student.FirstName) ||
                string.IsNullOrWhiteSpace(Student.LastName) ||
                string.IsNullOrWhiteSpace(Student.Email) ||
                string.IsNullOrWhiteSpace(Student.PostCode) ||
                Student.HouseNumber <= 0 ||
                string.IsNullOrWhiteSpace(Student.City))
            {
                return false;
            }

            if (!IsUniqueEmail(Student.Email))
            {
                ShowWarningDialog("Email address is al in gebruik.");
                return false;
            }

            return true;
        }

        private bool IsUniqueEmail(string email)
        {
            return !_studentRepository.EmailExists(email) || email == Student.Email;
        }

        private void ShowWarningDialog(string message)
        {
        }

        protected override void InvokeResponseCallback(DialogResult<Student> dialogResult)
        {
            ResponseCallback?.Invoke(dialogResult);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
