namespace CoursesManager.MVVM.Messages;

public abstract class BaseMessage<T> : ICloneable<T> where T : BaseMessage<T>
{
    public Guid MessageId { get; protected set; } = new();
    public DateTime TimeStamp { get; protected set; } = DateTime.Now;

    public abstract T Clone();
}