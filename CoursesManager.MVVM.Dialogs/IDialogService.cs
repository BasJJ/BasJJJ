using System.Windows;
using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Dialogs;

public interface IDialogService
{
    void RegisterDialog<TDialogViewModel, TDialogWindow, TDialogResponseType>(Func<TDialogViewModel> viewModelFactory)
        where TDialogViewModel : BaseDialogViewModel<TDialogResponseType>
        where TDialogResponseType : class
        where TDialogWindow : Window, new();

    Task<DialogResponse<TDialogResponseType>> ShowDialogAsync<TDialogViewModel, TDialogResponseType>()
        where TDialogViewModel : BaseDialogViewModel<TDialogResponseType>
        where TDialogResponseType : class;
}
