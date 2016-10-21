using System;
using System.Collections.Specialized;
using Xunit;

namespace ImageResizer.Test
{
    static class AssertEx
    {
        public static RaisedEvent<NotifyCollectionChangedEventArgs> Raises<T>(
            Action<NotifyCollectionChangedEventHandler> attach,
            Action<NotifyCollectionChangedEventHandler> detach,
            Action testCode)
            where T : NotifyCollectionChangedEventArgs
        {
            RaisedEvent<NotifyCollectionChangedEventArgs> raisedEvent = null;
            NotifyCollectionChangedEventHandler handler = (sender, e)
                => raisedEvent = new RaisedEvent<NotifyCollectionChangedEventArgs>(sender, e);
            attach(handler);
            testCode();
            detach(handler);

            Assert.NotNull(raisedEvent);

            return raisedEvent;
        }

        public class RaisedEvent<TArgs>
        {
            public RaisedEvent(object sender, TArgs args)
            {
                Sender = sender;
                Arguments = args;
            }

            public object Sender { get; }
            public TArgs Arguments { get; }
        }
    }
}
