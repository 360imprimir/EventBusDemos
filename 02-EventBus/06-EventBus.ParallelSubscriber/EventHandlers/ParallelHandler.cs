using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _06_EventBus.ParallelSubscriber.EventHandlers
{
    public class ParallelHandler : IEventSubscriberHandler<OrderCancelled>
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
