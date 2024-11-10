using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Dialogs.ResultTypes;

namespace CoursesManager.UI.Dialogs.ViewModels;

public class ConfirmationDialogViewModel : DialogViewModel<ConfirmationDialogResultType>
{
    private string _title = null!;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _message = null!;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ICommand ConfirmationCommand { get; private set; }

    public ConfirmationDialogViewModel(ConfirmationDialogResultType? initialData) : base(initialData)
    {
        if (initialData is not null)
        {
            Message = initialData.DialogText;
            Title = initialData.DialogTitle;
        }

        ConfirmationCommand = new RelayCommand(() =>
        {
            InvokeResponseCallback(DialogResult<ConfirmationDialogResultType>.Builder()
                .SetSuccess(new ConfirmationDialogResultType
                {
                    Result = false
                }).Build());
        });
    }

    protected override void InvokeResponseCallback(DialogResult<ConfirmationDialogResultType> dialogResult)
    {
        ResponseCallback?.Invoke(dialogResult);
    }
}
