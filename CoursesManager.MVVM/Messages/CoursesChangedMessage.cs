namespace CoursesManager.MVVM.Messages;

public class CoursesChangedMessage : BaseMessage<CoursesChangedMessage>
{
    public override CoursesChangedMessage Clone()
    {
        return new CoursesChangedMessage
        {
            MessageId = MessageId,
            TimeStamp = new(TimeStamp.Ticks)
        };
    }
}