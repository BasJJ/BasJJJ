using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Dialogs;

public abstract class DialogViewModel<TDialogResultType> : ViewModel
{
    protected Action<DialogResult<TDialogResultType>> ResponseCallback = null!;

    public void RegisterResponseCallback(Action<DialogResult<TDialogResultType>> responseCallback)
    {
        ResponseCallback = responseCallback;
    }
}