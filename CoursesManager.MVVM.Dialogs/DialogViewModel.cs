using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Dialogs;

/// <summary>
/// Base class for dialog view models that manage the result of a dialog operation.
/// </summary>
/// <typeparam name="TDialogResultType">The type of the result data returned by the dialog.</typeparam>
public abstract class DialogViewModel<TDialogResultType> : ViewModel
{
    /// <summary>
    /// Callback action to be invoked when the dialog is closed and a result is ready to be returned.
    /// </summary>
    protected Action<DialogResult<TDialogResultType>> ResponseCallback = null!;

    /// <summary>
    /// Registers a callback to handle the result of the dialog operation.
    /// </summary>
    /// <param name="responseCallback">The callback action that will be invoked with the <see cref="DialogResult{TDialogResultType}"/> when the dialog is closed.</param>
    public void RegisterResponseCallback(Action<DialogResult<TDialogResultType>> responseCallback)
    {
        ResponseCallback = responseCallback;
    }

    /// <summary>
    /// Method to invoke <see cref="ResponseCallback"/> so you can't forget.
    /// </summary>
    /// <param name="dialogResult">The result of the dialog.</param>
    protected abstract void InvokeResponseCallback(DialogResult<TDialogResultType> dialogResult);
}