using CoursesManager.MVVM.Data;
using System.Windows;

namespace CoursesManager.MVVM.Dialogs;

public sealed class DialogService : IDialogService
{
    private readonly Dictionary<Type, (Type windowType, Func<ViewModel> viewModelFactory)> _dialogMapping = new();

    public void RegisterDialog<TDialogViewModel, TDialogWindow, TDialogResultType>(Func<TDialogViewModel> viewModelFactory)
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogWindow : Window, new()
        where TDialogResultType : class
    {
        ArgumentNullException.ThrowIfNull(viewModelFactory);

        _dialogMapping[typeof(TDialogViewModel)] = (typeof(TDialogWindow), viewModelFactory);
    }

    public Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>()
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogResultType : class
    {
        return ShowDialogInternalAsync<TDialogViewModel, TDialogResultType>(null);
    }

    public Task<DialogResult<TDialogResultType>> ShowDialogAsync<TDialogViewModel, TDialogResultType>(TDialogResultType initialData)
        where TDialogViewModel : DialogViewModelInitialData<TDialogResultType>
        where TDialogResultType : class
    {
        return ShowDialogInternalAsync<TDialogViewModel, TDialogResultType>(initialData);
    }

    private Task<DialogResult<TDialogResultType>> ShowDialogInternalAsync<TDialogViewModel, TDialogResultType>(TDialogResultType? initialData)
        where TDialogViewModel : DialogViewModel<TDialogResultType>
        where TDialogResultType : class
    {
        if (!_dialogMapping.TryGetValue(typeof(TDialogViewModel), out var mappingTuple))
        {
            throw new InvalidOperationException($"No mapping registered for {typeof(TDialogViewModel).Name}");
        }

        var (windowType, viewModelFactory) = mappingTuple;

        var window = (Window)Activator.CreateInstance(windowType)!;
        var viewModel = (DialogViewModel<TDialogResultType>)viewModelFactory();

        if (initialData != null && viewModel is DialogViewModelInitialData<TDialogResultType> vm)
        {
            vm.SetInitialData(initialData);
        }

        window.DataContext = viewModel;

        var tcs = new TaskCompletionSource<DialogResult<TDialogResultType>>();

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

            var failureResponse = DialogResult<TDialogResultType>.Builder()
                .SetFailure("Dialog was closed by the user")
                .Build();

            tcs.SetResult(failureResponse);
        };

        window.ShowDialog();

        return tcs.Task;
    }
}