using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Dialogs.ResultTypes;

namespace CoursesManager.UI.Dialogs.ViewModels;

public class YesNoDialogViewModel : DialogViewModel<YesNoDialogResultType>
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

    public ICommand YesCommand { get; private set; }

    public ICommand NoCommand { get; private set; }

    public YesNoDialogViewModel(YesNoDialogResultType? initialData) : base(initialData)
    {
        if (initialData is not null)
        {
            Message = initialData.DialogText;
            Title = initialData.DialogTitle;
        }

        YesCommand = new RelayCommand(() =>
        {
            InvokeResponseCallback(DialogResult<YesNoDialogResultType>.Builder().SetSuccess(new YesNoDialogResultType
            {
                Result = true
            }).Build());
        });

        NoCommand = new RelayCommand(() =>
        {
            InvokeResponseCallback(DialogResult<YesNoDialogResultType>.Builder().SetSuccess(new YesNoDialogResultType
            {
                Result = false
            }).Build());
        });
    }

    protected override void InvokeResponseCallback(DialogResult<YesNoDialogResultType> dialogResult)
    {
        ResponseCallback.Invoke(dialogResult);
    }
}