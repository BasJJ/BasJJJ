namespace CoursesManager.MVVM.Messages;

internal interface IHandler<TMessageType>
{
    Action<TMessageType> Handler { get; set; }
}