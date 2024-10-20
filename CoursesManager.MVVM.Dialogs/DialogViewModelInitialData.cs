namespace CoursesManager.MVVM.Dialogs;

public abstract class DialogViewModelInitialData<TDialogResultType> : DialogViewModel<TDialogResultType>
{
    public abstract void SetInitialData(TDialogResultType initialData);
}