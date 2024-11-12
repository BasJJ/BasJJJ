using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoursesManager.UI.ViewModels.Courses
{
    class CourseOverViewViewModel : ViewModel
    {
        public ICommand ChangeCourseCommand { get; set; }

        public ICommand DeleteCourseCommand { get; set; }
        public Course CurrentCourse { get; set; }
        public ObservableCollection<Student>? Students { get; set; }

        public CourseOverViewViewModel()
        {
            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");
            Students = CurrentCourse.students;

        }


        private void ChangeCourse()
        {
            
        }

        private void DeleteCourse()
        {

        }
    }
}
