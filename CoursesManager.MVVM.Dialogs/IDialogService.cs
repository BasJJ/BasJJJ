using System.Windows;
using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Dialogs;

public interface IDialogService
{
    void RegisterDialog<TDialogViewModel, TDialogWindow, TDialogResultType>(Func<TDialogViewModel> viewModelFactory)
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogWindow : Window, new()
        where TDialogResultType : class;

    Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>()
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogResultType : class;

    Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>(TDialogResultType initialData)
        where TDialogViewModel : DialogViewModelInitialData<TDialogResultType>
        where TDialogResultType : class;
}
