using System;
using System.Threading;
using System.Threading.Tasks;
using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;

namespace _05_EventBus.SerialSubscriber.EventHandlers
{
    public class SerialHandler : IEventSubscriberHandler<OrderCancelled>
    {
        private int count = 0;

        public Task Handle(OrderCancelled @event, CancellationToken cancellationToken)
        {
            var currCount = Interlocked.Increment(ref count);
            Console.WriteLine($"[{DateTime.Now}]Processing event number: {currCount}");

            return Task.Delay(1000);
        }
    }
}
