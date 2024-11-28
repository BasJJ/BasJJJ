using CoursesManager.MVVM.Messages;
using CoursesManager.UI.Messages;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoursesManager.MVVM.Data;

public abstract class ViewModel : IsObservable, INotifyDataErrorInfo
{
    private bool _isDialogOpen;
    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set => SetProperty(ref _isDialogOpen, value);
    }

    private string? _viewTitle;
    public string? ViewTitle
    {
        get => _viewTitle;
        set => SetProperty(ref _viewTitle, value);
    }

    private Dictionary<string, List<string>> _allErrors = new();

    public bool HasErrors => _allErrors.Count != 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is not null && _allErrors.ContainsKey(propertyName)) return _allErrors[propertyName];

        return new List<string>();
    }

    public void AddError([CallerMemberName] string? propertyName = null, string errorMessage = "")
    {
        ArgumentNullException.ThrowIfNull(propertyName);

        if (!_allErrors.ContainsKey(propertyName)) _allErrors[propertyName] = new();

        _allErrors[propertyName].Add(errorMessage);
        ErrorsChanged?.Invoke(this, new(propertyName));
    }

    public void ClearErrors([CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);

        if (!_allErrors.ContainsKey(propertyName)) return;

        _allErrors.Remove(propertyName);
        ErrorsChanged?.Invoke(this, new(propertyName));
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public async Task ExecuteWithOverlayAsync(IMessageBroker _messageBroker, Func<Task> action)
    {
        _messageBroker.Publish(new OverlayActivationMessage(true));
        try
        {
            await action();
        }
        finally
        {
            _messageBroker.Publish(new OverlayActivationMessage(false));
        }
    }
}
