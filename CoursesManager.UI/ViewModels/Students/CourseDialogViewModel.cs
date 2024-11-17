using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.LocationRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoursesManager.UI.ViewModels.Students
{
    class CourseDialogViewModel : DialogViewModel<Course>
    {

        private readonly ICourseRepository _courseRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IDialogService _dialogService;
        private readonly ILocationRepository _locationRepository;

        public ICommand SaveCommand {  get;  }
        public ICommand CancelCommandd { get; }
        public Course Course { get; set; }


        public CourseDialogViewModel(ICourseRepository courseRepository,  IDialogService dialogService, ILocationRepository locationRepository, Course? course) : base(course)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));

            Course = course;
            SaveCommand = new RelayCommand(async () => await OnSaveAsync());
            CancelCommandd = new RelayCommand( OnCancel);

           
        }

        public CourseDialogViewModel(Course? dialogResultType) : base(dialogResultType)
        {
        }

        protected override void InvokeResponseCallback(DialogResult<Course> dialogResult)
        {
            ResponseCallback.Invoke(dialogResult);
        }

        private async Task OnSaveAsync()
        {

        }

        public void OnCancel()
        {
            var dialogResult = DialogResult<bool>.Builder()
                .SetSuccess(false, "Operation Cancelled")
                .Build();
            CloseDialogWithResult(dialogResult);
        }

        private void CloseDialogWithResult(DialogResult<bool> dialogResult)
        {
            throw new NotImplementedException();
        }
    }
}
