using CoursesManager.MVVM.Data;
using System.Windows;

namespace CoursesManager.MVVM.Dialogs;

public class DialogService : IDialogService
{
    private readonly Dictionary<Type, (Type windowType, Func<ViewModel> viewModelFactory)> _dialogMapping = new();

    public void RegisterDialog<TDialogViewModel, TDialogWindow, TDialogResponseType>(Func<TDialogViewModel> viewModelFactory) where TDialogViewModel : BaseDialogViewModel<TDialogResponseType> where TDialogWindow : Window, new() where TDialogResponseType : class
    {
        ArgumentNullException.ThrowIfNull(viewModelFactory);

        _dialogMapping[typeof(TDialogViewModel)] = (typeof(TDialogWindow), viewModelFactory);
    }

    public Task<DialogResponse<TDialogResponseType>> ShowDialogAsync<TDialogViewModel, TDialogResponseType>()
        where TDialogViewModel : BaseDialogViewModel<TDialogResponseType>
        where TDialogResponseType : class
    {
        if (!_dialogMapping.TryGetValue(typeof(TDialogViewModel), out var mappingTuple))
        {
            throw new InvalidOperationException($"No mapping registered for {typeof(TDialogViewModel).Name}");
        }

        var (windowType, viewModelFactory) = mappingTuple;

        var window = (Window)Activator.CreateInstance(windowType)!;
        var viewModel = (BaseDialogViewModel<TDialogResponseType>)viewModelFactory();

        window.DataContext = viewModel;

        var tcs = new TaskCompletionSource<DialogResponse<TDialogResponseType>>();

        viewModel.RegisterResponseCallback(response =>
        {
            if (!tcs.Task.IsCompleted)
            {
                tcs.SetResult(response);
            }
            window.Close();
        });

        window.Closed += (sender, args) =>
        {
            if (tcs.Task.IsCompleted) return;

            var failureResponse = DialogResponse<TDialogResponseType>.Builder()
                .SetFailure("Dialog was closed by the user")
                .Build();

            tcs.SetResult(failureResponse);
        };

        window.ShowDialog();

        return tcs.Task;
    }
}