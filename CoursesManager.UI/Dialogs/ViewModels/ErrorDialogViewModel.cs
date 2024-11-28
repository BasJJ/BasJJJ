using CoursesManager.UI.Dialogs.ResultTypes;

namespace CoursesManager.UI.Dialogs.ViewModels
{
    public class ErrorDialogViewModel(DialogResultType? initialData)
        : NotifyDialogViewModel(initialData);
}