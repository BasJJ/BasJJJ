using CoursesManager.MVVM.Commands;

namespace CoursesManager.MVVM.Tests.Commands;

public class RelayCommandGenericTests
{
    [Test]
    public void Constructor_ThrowsException_WhenExecuteDelegateIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RelayCommand<string>(null));
    }

    [Test]
    public void CanExecute_ReturnsTrue_WhenCanExecuteIsNull()
    {
        var command = new RelayCommand<string>(_ => { });

        bool result = command.CanExecute("test");

        Assert.IsTrue(result);
    }

    [Test]
    public void CanExecute_UsesPredicate_WhenCanExecuteIsProvided()
    {
        var command = new RelayCommand<string>(_ => { }, param => param == "valid");

        Assert.IsTrue(command.CanExecute("valid"));
        Assert.IsFalse(command.CanExecute("invalid"));
    }

    [Test]
    public void Execute_CallsAction_WhenParameterIsValid()
    {
        bool actionExecuted = false;
        var command = new RelayCommand<string>(_ => actionExecuted = true);

        command.Execute("test");

        Assert.IsTrue(actionExecuted);
    }

    [Test]
    public void Execute_ThrowsException_WhenParameterIsInvalid()
    {
        var command = new RelayCommand<string>(_ => { });

        Assert.Throws<ArgumentException>(() => command.Execute(123)); // 123 is an invalid type
    }

    [Test]
    public void CanExecute_ThrowsException_WhenParameterIsInvalid()
    {
        var command = new RelayCommand<string>(_ => { });

        Assert.Throws<ArgumentException>(() => command.CanExecute(123)); // 123 is an invalid type
    }

    [Test]
    public void Execute_CallsAction_WithCorrectParameter()
    {
        string expected = "expected";
        string actual = string.Empty;
        var command = new RelayCommand<string>(param => actual = param);

        command.Execute(expected);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void CanExecute_ReturnsFalse_WhenCanExecutePredicateIsFalse()
    {
        var command = new RelayCommand<int>(_ => { }, param => param > 10);

        bool result = command.CanExecute(5);

        Assert.IsFalse(result);
    }

    [Test]
    public void CanExecute_ReturnsTrue_WhenCanExecutePredicateIsTrue()
    {
        var command = new RelayCommand<int>(_ => { }, param => param > 10);

        bool result = command.CanExecute(15);

        Assert.IsTrue(result);
    }
}