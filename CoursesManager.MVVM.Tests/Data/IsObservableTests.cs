using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Tests.Data;

internal class TestOn : IsObservable
{
    private string _test = "initial";

    public string Test
    {
        get => _test;
        set => SetProperty(ref _test, value);
    }

    public bool PropertyChangedInvoked { get; set; }

    public TestOn()
    {
        PropertyChangedInvoked = false;

        PropertyChanged += (sender, args) =>
        {
            PropertyChangedInvoked = true;
        };
    }
}

public class IsObservableTests
{
    [Test]
    public void SetProperty_ChangesValue_WhenNewValueProvided()
    {
        var testOn = new TestOn();
        var expected = "new data";

        testOn.Test = expected;

        Assert.That(testOn.Test, Is.EqualTo(expected));
    }

    [Test]
    public void SetProperty_DoesNotInvokePropertyChanged_WhenValueDoesNotChange()
    {
        var testOn = new TestOn();

        testOn.Test = "initial";

        Assert.IsFalse(testOn.PropertyChangedInvoked);
    }

    [Test]
    public void SetProperty_InvokesPropertyChanged_WhenValueChanges()
    {
        var testOn = new TestOn();

        testOn.Test = "new";

        Assert.IsTrue(testOn.PropertyChangedInvoked);
    }
}