namespace CoursesManager.MVVM.Messages;

internal class Subscriber<TMessageType, TRecipient>(Action<TMessageType> handler, TRecipient recipient) : IHandler<TMessageType>, IRecipient<TRecipient>
{
    public Action<TMessageType> Handler { get; set; } = handler;

    public TRecipient Recipient { get; set; } = recipient;
}