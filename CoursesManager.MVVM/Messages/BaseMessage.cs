namespace CoursesManager.MVVM.Messages;

public abstract class BaseMessage
{
    public Guid MessageId { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public BaseMessage()
    {
        MessageId = new Guid();
        TimeStamp = DateTime.Now;
    }
}