using System.Windows;

namespace CoursesManager.MVVM.Dialogs;

public interface IDialogService
{
    /// <summary>
    /// Registers a dialog mapping with its window, viewmodel en dialog result types. All dialogs intended to be used should be registered.
    /// </summary>
    /// <typeparam name="TDialogViewModel">
    /// The type of the dialog view model, which should either be <see cref="DialogViewModel{TDialogResultType}"/> 
    /// or <see cref="DialogViewModelInitialData{TDialogResultType}"/>.
    /// </typeparam>
    /// <typeparam name="TDialogWindow">A WPF window to use as the dialog.</typeparam>
    /// <typeparam name="TDialogResultType">The type the dialog should return after it is done.</typeparam>
    /// <param name="viewModelFactory">A factory function to create instances of the view model. This enables support for injecting services with your approach.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="viewModelFactory"/> is null.</exception>
    void RegisterDialog<TDialogViewModel, TDialogWindow, TDialogResultType>(Func<TDialogViewModel> viewModelFactory)
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogWindow : Window, new()
        where TDialogResultType : class;

    /// <summary>
    /// Opens a dialog from the mapping based on the view model type provided.
    /// </summary>
    /// <typeparam name="TDialogViewModel">The type of the dialog view model, which should be <see cref="DialogViewModel{TDialogResultType}"/>.</typeparam>
    /// <typeparam name="TDialogResultType">The type of the dialog result data.</typeparam>
    /// <returns>A task representing the asynchronous operation, containing the result of the dialog.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there is no mapping registered for the specified view model type.</exception>
    Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>()
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogResultType : class;

    /// <summary>
    /// Opens a dialog from the mapping based on the view model type provided, with the ability to pass initial data.
    /// </summary>
    /// <typeparam name="TDialogViewModel">The type of the dialog view model, which should be <see cref="DialogViewModelInitialData{TDialogResultType}"/>.</typeparam>
    /// <typeparam name="TDialogResultType">The type of the dialog result data.</typeparam>
    /// <param name="initialData">The initial data to be passed to the dialog view model.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of the dialog.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there is no mapping registered for the specified view model type.</exception>
    Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>(TDialogResultType initialData)
        where TDialogViewModel : DialogViewModelInitialData<TDialogResultType>
        where TDialogResultType : class;
}
