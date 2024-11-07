namespace CoursesManager.MVVM.Data;

internal interface ICopyable<out T>
{
    T Copy();
}