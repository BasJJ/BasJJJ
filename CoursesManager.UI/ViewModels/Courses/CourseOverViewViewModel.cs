using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
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

        public CourseOverViewViewModel()
        {
            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");

            GlobalCache.Instance.RemovePermanentItem("Opened Course2");
            GlobalCache.Instance.RemovePermanentItem("Opened Course3");
            GlobalCache.Instance.RemovePermanentItem("Opened Course4");
            GlobalCache.Instance.RemovePermanentItem("Opened Course5");
            GlobalCache.Instance.RemovePermanentItem("Opened Course6");
            GlobalCache.Instance.RemovePermanentItem("Opened Course7");
            GlobalCache.Instance.RemovePermanentItem("Opened Course8");
            GlobalCache.Instance.RemovePermanentItem("Opened Course9");
            GlobalCache.Instance.RemovePermanentItem("Opened Course10");
            GlobalCache.Instance.RemovePermanentItem("Opened Course11");

            var cache = GlobalCache.Instance;
            Debug.WriteLine($"Current capacity: {cache.CurrentCapacity}");

        }


        private void ChangeCourse()
        {
            
        }

        private void DeleteCourse()
        {

        }
    }
}
