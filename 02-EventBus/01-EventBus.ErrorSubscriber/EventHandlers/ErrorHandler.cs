using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace _01_EventBus.ErrorSubscriber.EventHandlers
{
    public class ErrorHandler : IEventSubscriberHandler<OrderCancelled>
    {
        public Task Handle(OrderCancelled @event, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
