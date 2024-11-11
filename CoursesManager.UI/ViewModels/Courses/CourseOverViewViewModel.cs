using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CourseOverViewViewModel()
        {
            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");

        }


        private void ChangeCourse()
        {
            
        }

        private void DeleteCourse()
        {

        }
    }
}
