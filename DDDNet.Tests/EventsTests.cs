using DDDNet.Events;
using DDDNet.Infrastructure.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDDNet.Tests
{
    [TestClass]
    public class EventsTests
    {
        class UserCreated : IEvent { }
        class PasswordChanged : IEvent { }

        class User : EventSource
        {
            public User()
            {
                RaiseEvent(new UserCreated());
            }

            public void ChangePassword()
            {
                RaiseEvent(new PasswordChanged());
            }
        }

        [TestMethod]
        public void TestEventSource()
        {
            var src = new User();

            Assert.AreEqual(1, src.PopEvents().Length);
            Assert.AreEqual(0, src.PopEvents().Length);

            src = new User();

            Assert.IsInstanceOfType(src.PopEvents()[0], typeof(UserCreated));

            src = new User();
            src.ChangePassword();

            var evts = src.PopEvents();

            Assert.AreEqual(2, evts.Length);
            Assert.IsInstanceOfType(evts[0], typeof(UserCreated));
            Assert.IsInstanceOfType(evts[1], typeof(PasswordChanged));
        }

        [TestMethod]
        public void TestEventDispatcher()
        {
            var dispatcher = new ImmediateDispatcher();

            var numberOfHandlersCalled = 0;

            dispatcher.Handle<UserCreated>(e =>
            {
                ++numberOfHandlersCalled;
            });

            dispatcher.Handle<PasswordChanged>(e =>
            {
                ++numberOfHandlersCalled;
            });

            var src = new User();
            src.ChangePassword();

            dispatcher.Dispatch(src.PopEvents());

            Assert.AreEqual(2, numberOfHandlersCalled);

            dispatcher.Dispatch(src.PopEvents());

            Assert.AreEqual(2, numberOfHandlersCalled);

            dispatcher.Handle<PasswordChanged>(e =>
            {
                ++numberOfHandlersCalled;
            });

            src.ChangePassword();

            dispatcher.Dispatch(src.PopEvents());

            Assert.AreEqual(4, numberOfHandlersCalled);
        }
    }
}
