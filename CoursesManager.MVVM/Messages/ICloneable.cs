namespace CoursesManager.MVVM.Messages;

public interface ICloneable<out T>
{
    T Clone();
}