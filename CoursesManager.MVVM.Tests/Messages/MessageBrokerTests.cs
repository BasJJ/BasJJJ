using CoursesManager.MVVM.Messages;

namespace CoursesManager.MVVM.Tests.Messages;

internal class TestMessageOne : BaseMessage<TestMessageOne>
{
    public override TestMessageOne Clone()
    {
        return new TestMessageOne
        {
            MessageId = MessageId,
            TimeStamp = new(TimeStamp.Ticks)
        };
    }
}

internal class TestMessageTwo : BaseMessage<TestMessageTwo>
{
    public override TestMessageTwo Clone()
    {
        return new TestMessageTwo
        {
            MessageId = MessageId,
            TimeStamp = new(TimeStamp.Ticks)
        };
    }
}

public class MessageBrokerTests
{
    private IMessageBroker _messageBroker;

    [SetUp]
    public void SetUp()
    {
        _messageBroker = new MessageBroker();
    }

    [Test]
    public void Subscribe_ThrowsArgumentNullException_WhenHandlerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _messageBroker.Subscribe<TestMessageOne>(null);
        });
    }

    [Test]
    public void Publish_ThrowsArgumentNullException_WhenMessageIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _messageBroker.Publish<TestMessageOne>(null);
        });
    }

    [Test]
    public void Publish_InvokesOnlyTestMessageOne_WhenMessageIsProvided()
    {
        var shouldBeInvoked = false;
        var shouldNotBeInvoked = true;

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            shouldBeInvoked = true;
        });

        _messageBroker.Subscribe<TestMessageTwo>(_ =>
        {
            shouldNotBeInvoked = false;
        });

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(shouldBeInvoked);
        Assert.IsTrue(shouldNotBeInvoked);
    }

    [Test]
    public void Publish_InvokesOnlyTestMessageTwo_WhenMessageIsProvided()
    {
        var shouldBeInvoked = false;
        var shouldNotBeInvoked = true;

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            shouldNotBeInvoked = false;
        });

        _messageBroker.Subscribe<TestMessageTwo>(_ =>
        {
            shouldBeInvoked = true;
        });

        _messageBroker.Publish(new TestMessageTwo());

        Assert.IsTrue(shouldBeInvoked);
        Assert.IsTrue(shouldNotBeInvoked);
    }

    [Test]
    public void Publish_SubscribersGetClonedMessage()
    {
        TestMessageTwo subscriberOneMessage = null;
        TestMessageTwo subscriberTwoMessage = null;

        _messageBroker.Subscribe<TestMessageTwo>(message =>
        {
            subscriberOneMessage = message;
        });

        _messageBroker.Subscribe<TestMessageTwo>(message =>
        {
            subscriberTwoMessage = message;
        });

        _messageBroker.Publish(new TestMessageTwo());

        Assert.IsFalse(ReferenceEquals(subscriberOneMessage, subscriberTwoMessage));
    }

    [Test]
    public void Publish_ReturnsFalse_WhenNoSubscribers()
    {
        Assert.IsFalse(_messageBroker.Publish(new TestMessageOne()));
    }

    [Test]
    public void Publish_ReturnsTrue_WhenThereAreSubscribers()
    {
        _messageBroker.Subscribe<TestMessageOne>(_ => { });

        Assert.IsTrue(_messageBroker.Publish(new TestMessageOne()));
    }

    [Test]
    public void Publish_AllSubscribersInvoked()
    {
        var shouldBeInvoked = true;
        var shouldAlsoBeInvoked = true;

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            shouldBeInvoked = true;
        });

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            shouldAlsoBeInvoked = true;
        });

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(shouldBeInvoked);
        Assert.IsTrue(shouldAlsoBeInvoked);
    }

    [Test]
    public void Unsubscribe_ThrowsArgumentNullException_WhenHandlerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _messageBroker.Unsubscribe<TestMessageOne>(null);
        });
    }

    [Test]
    public void Unsubscribe_HandlerNotInvoked_WhenUnsubscribed()
    {
        var notInvoked = true;

        void Handler(TestMessageOne obj)
        {
            notInvoked = false;
        }

        _messageBroker.Subscribe<TestMessageOne>(Handler);

        _messageBroker.Unsubscribe<TestMessageOne>(Handler);

        Assert.IsTrue(notInvoked);
    }

    [Test]
    public void Unsubscribe_NotAllHandlersInvoked_WhenOneUnsubscribed()
    {
        var handlerOneNotInvoked = true;
        var handlerTwoInvoked = false;

        void HandlerOne(TestMessageOne obj)
        {
            handlerOneNotInvoked = false;
        }

        void HandlerTwo(TestMessageOne obj)
        {
            handlerTwoInvoked = true;
        }

        _messageBroker.Subscribe<TestMessageOne>(HandlerOne);
        _messageBroker.Subscribe<TestMessageOne>(HandlerTwo);

        _messageBroker.Unsubscribe<TestMessageOne>(HandlerOne);

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(handlerOneNotInvoked);
        Assert.IsTrue(handlerTwoInvoked);
    }

    [Test]
    public void UnsubscribeMe_ThrowsArgumentNullException_WhenNoRecipientProvided()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _messageBroker.UnsubscribeMe<MessageBrokerTests>(null);
        });
    }

    [Test]
    public void UnsubscribeMe_NoHandlersInvoked_WhenUnsubscribed()
    {
        var handlerOneNotInvoked = true;
        var handlerTwoNotInvoked = true;

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            handlerOneNotInvoked = false;
        });

        _messageBroker.Subscribe<TestMessageOne>(_ =>
        {
            handlerTwoNotInvoked = false;
        });

        _messageBroker.UnsubscribeMe(this);

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(handlerOneNotInvoked);
        Assert.IsTrue(handlerTwoNotInvoked);
    }

    [Test]
    public void Publish_Concurrency_WhenManyPublishesAreMade()
    {
    }

    [Test]
    public void UnsubscribeAll_DoesAsStated()
    {
    }
}