namespace CoursesManager.MVVM.Messages;

public abstract class BaseMessage
{
    public Guid MessageId { get; private set; } = new();
    public DateTime TimeStamp { get; private set; } = DateTime.Now;
}