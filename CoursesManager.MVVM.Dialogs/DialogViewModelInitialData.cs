namespace CoursesManager.MVVM.Dialogs;

/// <summary>
/// Base class for dialog view models that require initial data to be set before showing the dialog.
/// </summary>
/// <typeparam name="TDialogResultType">The type of the result data returned by the dialog.</typeparam>
public abstract class DialogViewModelInitialData<TDialogResultType> : DialogViewModel<TDialogResultType>
{
    /// <summary>
    /// Sets the initial data required by the dialog before it is displayed.
    /// </summary>
    /// <param name="initialData">The initial data to be passed to the dialog view model.</param>
    public abstract void SetInitialData(TDialogResultType initialData);
}