namespace CoursesManager.MVVM.Dialogs;

public sealed class DialogResponse<TDialogResponseType> where TDialogResponseType : class
{
    public bool IsSuccess { get; private set; }
    public string? FailureMessage { get; private set; }
    public TDialogResponseType? Data { get; private set; }

    private DialogResponse() { }

    public static DialogResponseBuilder Builder() => new();

    public class DialogResponseBuilder
    {
        private readonly DialogResponse<TDialogResponseType> _dialogResponse = new();

        public DialogResponseBuilder SetSuccess(TDialogResponseType data)
        {
            _dialogResponse.Data = data;
            _dialogResponse.IsSuccess = true;
            _dialogResponse.FailureMessage = null;
            return this;
        }

        public DialogResponseBuilder SetFailure(string failureMessage)
        {
            _dialogResponse.Data = default(TDialogResponseType);
            _dialogResponse.IsSuccess = false;
            _dialogResponse.FailureMessage = failureMessage;
            return this;
        }

        public DialogResponse<TDialogResponseType> Build()
        {
            return _dialogResponse;
        }
    }
}