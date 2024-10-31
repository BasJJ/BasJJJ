using CoursesManager.MVVM.Commands;

namespace CoursesManager.MVVM.Tests.Commands;

public class RelayCommandTests
{
    [Test]
    public void Constructor_ThrowsException_WhenExecuteDelegateIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var command = new RelayCommand(null);
        });
    }

    [Test]
    public void CanExecute_ReturnsTrue_WhenCanExecuteIsNull()
    {
        var command = new RelayCommand(() => { });

        var canExecute = command.CanExecute(null);

        Assert.IsTrue(canExecute);
    }

    [Test]
    public void CanExecute_ReturnsFalse_WhenCanExecuteShouldReturnFalse()
    {
        var command = new RelayCommand(() => { }, () => false);

        var canExecute = command.CanExecute(null);

        Assert.IsFalse(canExecute);
    }

    [Test]
    public void Execute_CallsAction()
    {
        var executed = false;
        var command = new RelayCommand(() => { executed = true; });

        command.Execute(null);

        Assert.IsTrue(executed);
    }
}