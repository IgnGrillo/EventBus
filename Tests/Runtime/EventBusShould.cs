using System;
using NUnit.Framework;

namespace Tests.Editor
{
    public class EventBusShould
    {
        private static int _callbackCounter;

        [SetUp]
        public void SetUp()
        {
            EventBus.Clear();
            _callbackCounter = 0;
        }

        [Test]
        public void InvokeTheActionProvidedWhenSubscribedAndPublished()
        {
            var emitableMockAction = GivenAnActionThatAddsACounterWhenCalled();
            GivenAnEventBusSubscriptionOfAnAction(emitableMockAction);
            WhenAnEmitableMockIsPublished();
            ThenCallbackCounterWasAugmented(1);
        }
        
        [Test]
        public void NotThrowAnErrorWhenPublishingAnEmitableNeverSubscribed()
        {
            try
            {
                WhenAnEmitableMockIsPublished();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        
        [Test]
        public void InvokeTheActionsProvidedWhenSubscribedAndPublished()
        {
            var emitableMockAction = GivenAnActionThatAddsACounterWhenCalled();
            GivenAnEventBusSubscriptionOfAnAction(emitableMockAction);
            GivenAnEventBusSubscriptionOfAnAction(emitableMockAction);
            GivenAnEventBusSubscriptionOfAnAction(emitableMockAction);
            WhenAnEmitableMockIsPublished();
            ThenCallbackCounterWasAugmented(3);
        }

        [Test]
        public void NotCallAnActionThatWasSubscribedAndLaterRemovedWhenPublishing()
        {
            var emitableMockAction = GivenAnActionThatAddsACounterWhenCalled();
            var subscriptionGuid = GivenAnEventBusSubscriptionOfAnAction(emitableMockAction);
            EventBus.RemoveSubscription<EmitableMock>(subscriptionGuid);
            WhenAnEmitableMockIsPublished();
            ThenCallbackCounterWasNotAugmented();
        }
        
        [Test]
        public void NotThrowAnErrorWhenRemovingANonExistingAction()
        {
            try
            {
                WhenRemovingANonExistentAction();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        private static Action<EmitableMock> GivenAnActionThatAddsACounterWhenCalled() =>
                AnEmitableMockThatInvokes(() => _callbackCounter++);

        private static Guid GivenAnEventBusSubscriptionOfAnAction(Action<EmitableMock> emitableMockAction) =>
                EventBus.Subscribe(emitableMockAction);

        private static void WhenAnEmitableMockIsPublished() =>
                EventBus.Publish(new EmitableMock());
        
        private static void WhenRemovingANonExistentAction() =>
                EventBus.RemoveSubscription<EmitableMock>(Guid.NewGuid());

        private static void ThenCallbackCounterWasAugmented(int expected) =>
                Assert.AreEqual(expected, _callbackCounter);
        
        private static void ThenCallbackCounterWasNotAugmented() =>
                Assert.AreEqual(0, _callbackCounter);

        private static Action<EmitableMock> AnEmitableMockThatInvokes(Action actionToCall) =>
                _ => actionToCall.Invoke();

        private class EmitableMock : Emitable { }
    }
}