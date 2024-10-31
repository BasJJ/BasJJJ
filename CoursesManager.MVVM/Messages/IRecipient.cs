namespace CoursesManager.MVVM.Messages;

internal interface IRecipient<TRecipient>
{
    TRecipient Recipient { get; set; }
}