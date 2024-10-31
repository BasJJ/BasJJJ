namespace CoursesManager.MVVM.Messages;

/// <summary>
/// Interface for a message broker that handles publishing and subscribing to messages in an application.
/// </summary>
public interface IMessageBroker
{
    /// <summary>
    /// Publishes a message to all subscribers.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message being published, which must inherit from <see cref="BaseMessage"/>.</typeparam>
    /// <param name="message">The message instance to publish.</param>
    /// <returns><c>true</c> if the message was published successfully; otherwise, <c>false</c> which means nobody was listening.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is <c>null</c>.</exception>
    bool Publish<TMessageType>(TMessageType message) where TMessageType : BaseMessage<TMessageType>;

    /// <summary>
    /// Subscribes to a specific message type by providing a handler to process the message when it is published.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message to subscribe to, which must inherit from <see cref="BaseMessage"/>.</typeparam>
    /// <param name="handler">The action to invoke when a message of type <typeparamref name="TMessageType"/> is published.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler"/> is <c>null</c>.</exception>
    void Subscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage<TMessageType>;

    /// <summary>
    /// Unsubscribes a previously registered handler from a specific message type.
    /// </summary>
    /// <typeparam name="TMessageType">The type of message to unsubscribe from, which must inherit from <see cref="BaseMessage"/>.</typeparam>
    /// <param name="handler">The handler that was previously registered with <see cref="Subscribe{TMessageType}(Action{TMessageType})"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler"/> is <c>null</c>.</exception>
    void Unsubscribe<TMessageType>(Action<TMessageType> handler) where TMessageType : BaseMessage<TMessageType>;

    /// <summary>
    /// Unsubscribes all handlers for a specific recipient from all message types.
    /// </summary>
    /// <typeparam name="TRecipient">The type of the recipient that is unsubscribing, typically a class.</typeparam>
    /// <param name="me">The recipient instance to unsubscribe.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="me"/> is <c>null</c>.</exception>
    void UnsubscribeMe<TRecipient>(TRecipient me) where TRecipient : class;

    /// <summary>
    /// Unsubscribes all registered handlers from all message types.
    /// </summary>
    void UnsubscribeAll();
}