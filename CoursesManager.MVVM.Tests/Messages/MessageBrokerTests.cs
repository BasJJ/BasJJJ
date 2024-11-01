using CoursesManager.MVVM.Messages;
using NUnit.Framework.Internal;

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
            _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(null, this);
        });
    }

    [Test]
    public void Subscribe_ThrowsArgumentNullException_WhenRecipientIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ => { }, null);
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

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            shouldBeInvoked = true;
        }, this);

        _messageBroker.Subscribe<TestMessageTwo, MessageBrokerTests>(_ =>
        {
            shouldNotBeInvoked = false;
        }, this);

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(shouldBeInvoked);
        Assert.IsTrue(shouldNotBeInvoked);
    }

    [Test]
    public void Publish_InvokesOnlyTestMessageTwo_WhenMessageIsProvided()
    {
        var shouldBeInvoked = false;
        var shouldNotBeInvoked = true;

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            shouldNotBeInvoked = false;
        }, this);

        _messageBroker.Subscribe<TestMessageTwo, MessageBrokerTests>(_ =>
        {
            shouldBeInvoked = true;
        }, this);

        _messageBroker.Publish(new TestMessageTwo());

        Assert.IsTrue(shouldBeInvoked);
        Assert.IsTrue(shouldNotBeInvoked);
    }

    [Test]
    public void Publish_SubscribersGetClonedMessage()
    {
        TestMessageTwo subscriberOneMessage = null;
        TestMessageTwo subscriberTwoMessage = null;

        _messageBroker.Subscribe<TestMessageTwo, MessageBrokerTests>(message =>
        {
            subscriberOneMessage = message;
        }, this);

        _messageBroker.Subscribe<TestMessageTwo, MessageBrokerTests>(message =>
        {
            subscriberTwoMessage = message;
        }, this);

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
        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ => { }, this);

        Assert.IsTrue(_messageBroker.Publish(new TestMessageOne()));
    }

    [Test]
    public void Publish_AllSubscribersInvoked()
    {
        var shouldBeInvoked = true;
        var shouldAlsoBeInvoked = true;

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            shouldBeInvoked = true;
        }, this);

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            shouldAlsoBeInvoked = true;
        }, this);

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

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(Handler, this);

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

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(HandlerOne, this);
        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(HandlerTwo, this);

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

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            handlerOneNotInvoked = false;
        }, this);

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            handlerTwoNotInvoked = false;
        }, this);

        _messageBroker.UnsubscribeMe(this);

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(handlerOneNotInvoked);
        Assert.IsTrue(handlerTwoNotInvoked);
    }

    [Test]
    public async Task Publish_Concurrency_WhenManyPublishesAreMade()
    {
        var message = new TestMessageOne();

        int received = 0;
        int totalSubscribers = 1000;
        int amountOfPublishes = 100;
        int expectedReceived = totalSubscribers * amountOfPublishes;

        Parallel.For(0, totalSubscribers, _ =>
        {
            _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ => Interlocked.Increment(ref received), this);
        });

        var tasks = new List<Task>();
        for (int i = 0; i < amountOfPublishes; i++)
        {
            tasks.Add(Task.Run(() => _messageBroker.Publish(message)));
        }

        await Task.WhenAll(tasks);

        Assert.That(received, Is.EqualTo(expectedReceived));
    }

    [Test]
    public void UnsubscribeAll_DoesAsStated()
    {
        var handlerOneNotInvoked = true;
        var handlerTwoNotInvoked = true;

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            handlerOneNotInvoked = false;
        }, this);

        _messageBroker.Subscribe<TestMessageOne, MessageBrokerTests>(_ =>
        {
            handlerTwoNotInvoked = false;
        }, this);

        _messageBroker.UnsubscribeAll();

        _messageBroker.Publish(new TestMessageOne());

        Assert.IsTrue(handlerOneNotInvoked);
        Assert.IsTrue(handlerTwoNotInvoked);
    }

}