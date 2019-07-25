using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _04_EventBus.ResilientSubscriber.EventHandlers
{
    public class RetryHandler : IEventSubscriberHandler<OrderCancelled>
    {
        private readonly Random random = new Random();

        public Task Handle(OrderCancelled @event, CancellationToken cancellationToken)
        {
            var value = this.random.Next(100);
            if(value > 50)
            {
                throw new Exception("Exception processing the order cancelled event.");
            }

            return Task.CompletedTask;
        }
    }
}
