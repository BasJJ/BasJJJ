namespace CoursesManager.MVVM.Dialogs;

public sealed class DialogResult<TDialogResultType>
{
    public DialogOutcome Outcome { get; private set; }
    public string? OutcomeMessage { get; private set; }

    public TDialogResultType? Data { get; private set; }

    private DialogResult() { }

    public static DialogResultBuilder Builder() => new();

    public class DialogResultBuilder
    {
        private readonly DialogResult<TDialogResultType> _dialogResult = new();

        public DialogResultBuilder SetOutcome(DialogOutcome outcome)
        {
            _dialogResult.Outcome = outcome;
            return this;
        }

        public DialogResultBuilder SetData(TDialogResultType data)
        {
            _dialogResult.Data = data;
            return this;
        }

        public DialogResultBuilder SetOutcomeMessage(string outcomeMessage)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(outcomeMessage);

            _dialogResult.OutcomeMessage = outcomeMessage;
            return this;
        }

        public DialogResultBuilder SetSuccess(TDialogResultType data, string? outcomeMessage)
        {
            _dialogResult.Data = data;
            _dialogResult.OutcomeMessage = outcomeMessage;
            _dialogResult.Outcome = DialogOutcome.Success;
            return this;
        }

        public DialogResultBuilder SetFailure(string outcomeMessage)
        {
            SetOutcomeMessage(outcomeMessage);
            _dialogResult.Outcome = DialogOutcome.Failure;
            return this;
        }

        public DialogResultBuilder SetCanceled(string outcomeMessage)
        {
            SetOutcomeMessage(outcomeMessage);
            _dialogResult.Outcome = DialogOutcome.Canceled;
            return this;
        }

        public DialogResult<TDialogResultType> Build()
        {
            return _dialogResult;
        }
    }
}