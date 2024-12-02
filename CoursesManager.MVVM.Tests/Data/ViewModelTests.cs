using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Tests.Data;

internal class TestViewModel : ViewModel
{
}

public class ViewModelTests
{
    private TestViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _viewModel = new TestViewModel();
    }

    [Test]
    public void AddError_AddsErrorMessageToProperty()
    {
        var propertyName = "TestProperty";
        var errorMessage = "This is an error.";

        _viewModel.AddError(propertyName, errorMessage);

        Assert.That(_viewModel.HasErrors, Is.True);
        var errors = _viewModel.GetErrors(propertyName) as List<string>;
        Assert.That(errors, Is.Not.Null);
        Assert.That(errors, Contains.Item(errorMessage));
    }

    [Test]
    public void ClearErrors_RemovesAllErrorsFromProperty()
    {
        var propertyName = "TestProperty";
        _viewModel.AddError(propertyName, "Error 1");
        _viewModel.AddError(propertyName, "Error 2");

        _viewModel.ClearErrors(propertyName);

        Assert.That(_viewModel.HasErrors, Is.False);
        var errors = _viewModel.GetErrors(propertyName) as List<string>;
        Assert.That(errors, Is.Not.Null);
        Assert.That(errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetErrors_ReturnsEmptyList_WhenNoErrors()
    {
        var propertyName = "NonExistentProperty";

        var errors = _viewModel.GetErrors(propertyName) as List<string>;

        Assert.That(errors, Is.Not.Null);
        Assert.That(errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void ErrorsChanged_EventIsRaised_WhenErrorIsAdded()
    {
        var propertyName = "TestProperty";
        bool eventRaised = false;
        _viewModel.ErrorsChanged += (sender, args) =>
        {
            if (args.PropertyName == propertyName)
            {
                eventRaised = true;
            }
        };

        _viewModel.AddError(propertyName, "Error message");

        Assert.That(eventRaised, Is.True);
    }

    [Test]
    public void ErrorsChanged_EventIsRaised_WhenErrorIsCleared()
    {
        var propertyName = "TestProperty";
        _viewModel.AddError(propertyName, "Error message");
        bool eventRaised = false;
        _viewModel.ErrorsChanged += (sender, args) =>
        {
            if (args.PropertyName == propertyName)
            {
                eventRaised = true;
            }
        };

        _viewModel.ClearErrors(propertyName);

        Assert.That(eventRaised, Is.True);
    }
}