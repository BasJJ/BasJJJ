namespace CoursesManager.MVVM.Messages;

public interface IMessageBroker
{
    bool Publish<TMessageType>(TMessageType message) where TMessageType : BaseMessage;

    void Subscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage;

    void Unsubscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage;

    void UnsubscribeMe<TRecipient>(TRecipient me) where TRecipient : class;

    void UnsubscribeAll();
}