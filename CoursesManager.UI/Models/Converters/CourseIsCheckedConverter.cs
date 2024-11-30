using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Converters
{
    public class CourseIsCheckedConverter : IValueConverter
    {
        /// <summary>
        /// Determines if the course is selected by checking if it is in the SelectedCourses list.
        /// </summary>
        /// <param name="value">The course object from the list of all courses.</param>
        /// <param name="parameter">The list of selected courses.</param>
        /// <returns>True if the course is selected; otherwise, false.</returns>
        public object IsCourseSelected(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Course course && parameter is ObservableCollection<Course> selectedCourses)
            {
                return selectedCourses.Any(c => c.Id == course.Id); // Check if course is selected
            }
            return false;
        }

        /// <summary>
        /// Adds or removes the course from the SelectedCourses list based on the checkbox state.
        /// </summary>
        /// <param name="value">The checkbox state (true if checked, false if unchecked).</param>
        /// <param name="parameter">The course to add or remove.</param>
        /// <returns>Returns Binding.DoNothing to avoid unintended binding changes.</returns>
        public object UpdateSelectedCourses(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && parameter is Course course && parameter is ObservableCollection<Course> selectedCourses)
            {
                if (isChecked)
                {
                    if (!selectedCourses.Any(c => c.Id == course.Id))
                    {
                        selectedCourses.Add(course); // Add course if checkbox is checked
                    }
                }
                else
                {
                    selectedCourses.Remove(selectedCourses.FirstOrDefault(c => c.Id == course.Id)); // Remove course if unchecked
                }
            }
            return Binding.DoNothing;
        }

        // Implement the IValueConverter interface explicitly to use renamed methods
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            IsCourseSelected(value, targetType, parameter, culture);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            UpdateSelectedCourses(value, targetType, parameter, culture);
    }
}
