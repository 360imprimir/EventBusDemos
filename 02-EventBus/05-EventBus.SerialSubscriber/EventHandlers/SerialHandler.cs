using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _05_EventBus.SerialSubscriber.EventHandlers
{
    public class SerialHandler : IEventSubscriberHandler<OrderCancelled>
    {
        private int count = 0;

        public Task Handle(OrderCancelled @event, CancellationToken cancellationToken)
        {
            var currCount = Interlocked.Increment(ref count);
            Console.WriteLine($"Processing event number: {currCount}");

            return Task.Delay(1000);
        }
    }
}
