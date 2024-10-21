namespace CoursesManager.MVVM.Messages;

public class MessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<object>> _subscribers = new();

    public bool Publish<TMessageType>(TMessageType message) where TMessageType : BaseMessage
    {
        ArgumentNullException.ThrowIfNull(message);

        lock (_subscribers)
        {
            if (!_subscribers.TryGetValue(typeof(TMessageType), out var subscriber)) return false;

            var subscriberList = subscriber.ToList();

            subscriberList.ForEach(a => ((Action<TMessageType>)a)?.Invoke(message));
            return true;
        }
    }

    public void Subscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (_subscribers)
        {
            if (!_subscribers.ContainsKey(typeof(TMessageType)))
            {
                _subscribers[typeof(TMessageType)] = new();
            }

            _subscribers[typeof(TMessageType)].Add(handler);
        }
    }

    public void Unsubscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (_subscribers)
        {
            if (!_subscribers.ContainsKey(typeof(TMessageType))) return;

            _subscribers[typeof(TMessageType)].RemoveAll(h => h.Equals(handler));
        }
    }

    public void UnsubscribeMe<TRecipient>(TRecipient me) where TRecipient : class
    {
        ArgumentNullException.ThrowIfNull(me);

        lock (_subscribers)
        {
            foreach (var subscriberList in _subscribers.Values)
            {
                subscriberList.RemoveAll(handler => handler is Delegate del && del.Target == me);
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