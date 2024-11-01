namespace CoursesManager.MVVM.Messages;

public class MessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<object>> _subscribers = new();

    public bool Publish<TMessageType>(TMessageType message) 
        where TMessageType : BaseMessage<TMessageType>
    {
        ArgumentNullException.ThrowIfNull(message);

        lock (_subscribers)
        {
            if (!_subscribers.TryGetValue(typeof(TMessageType), out var subscriberList)) return false;

            subscriberList.ForEach(subscriber =>
            {
                if (subscriber is IHandler<TMessageType> handler)
                {
                    handler.Handler.Invoke(message.Clone());
                }
            });

            return true;
        }
    }

    public void Subscribe<TMessageType, TRecipient>(Action<TMessageType> handler, TRecipient recipient)
        where TMessageType : BaseMessage<TMessageType>
        where TRecipient : class
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(recipient);

        lock (_subscribers)
        {
            if (!_subscribers.ContainsKey(typeof(TMessageType)))
            {
                _subscribers[typeof(TMessageType)] = new();
            }

            _subscribers[typeof(TMessageType)].Add(new Subscriber<TMessageType, TRecipient>(handler, recipient));
        }
    }

    public void Unsubscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage<TMessageType>
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (_subscribers)
        {
            if (!_subscribers.ContainsKey(typeof(TMessageType))) return;

            _subscribers[typeof(TMessageType)].RemoveAll(subscriber => subscriber is IHandler<TMessageType> h && h.Handler.Equals(handler));
        }
    }

    public void UnsubscribeMe<TRecipient>(TRecipient me)
        where TRecipient : class
    {
        ArgumentNullException.ThrowIfNull(me);

        lock (_subscribers)
        {
            foreach (var subscribersList in _subscribers.Values)
            {
                subscribersList.RemoveAll(subscriber => subscriber is IRecipient<TRecipient> sub && sub.Recipient.Equals(me));
            }
        }
    }
    
    public void UnsubscribeAll()
    {
        lock (_subscribers)
        {
            _subscribers.Clear();
        }
    }
}