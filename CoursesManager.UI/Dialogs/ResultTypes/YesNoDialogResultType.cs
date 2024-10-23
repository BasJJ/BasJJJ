using CoursesManager.MVVM.Data;

namespace CoursesManager.UI.Dialogs.ResultTypes;

public class YesNoDialogResultType : ViewModel
{
    public bool Result { get; set; }

    public string DialogText { get; set; }
    public string DialogTitle { get; set; }
}