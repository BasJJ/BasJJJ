using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Dialogs;

public abstract class BaseDialogViewModel<TDialogResponseType> : ViewModel where TDialogResponseType : class
{
    protected Action<DialogResponse<TDialogResponseType>> ResponseCallback = null!;

    public void RegisterResponseCallback(Action<DialogResponse<TDialogResponseType>> responseCallback)
    {
        ResponseCallback = responseCallback;
    }
}