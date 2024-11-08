using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Dialogs.ResultTypes;

namespace CoursesManager.UI.Dialogs.ViewModels;

public class OkDialogViewModel : DialogViewModel<OkDialogResultType>
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

    public ICommand OkCommand { get; private set; }

    public OkDialogViewModel(OkDialogResultType? initialData) : base(initialData)
    {
        if (initialData is not null)
        {
            Message = initialData.DialogText;
            Title = initialData.DialogTitle;
        }

        OkCommand = new RelayCommand(() =>
        {
            InvokeResponseCallback(DialogResult<OkDialogResultType>.Builder()
                .SetSuccess(new OkDialogResultType
                {
                    Result = false
                }).Build());
        });
    }

    protected override void InvokeResponseCallback(DialogResult<OkDialogResultType> dialogResult)
    {
        ResponseCallback?.Invoke(dialogResult);
    }
}
