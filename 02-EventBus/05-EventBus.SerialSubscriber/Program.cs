using System;
using System.Threading.Tasks;
using _00_Contract;
using _05_EventBus.SerialSubscriber.EventHandlers;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _05_EventBus.SerialSubscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SerialHandler>();

                services.AddRabbitMqEventBus(
                    hostContext.HostingEnvironment.EnvironmentName,
                    "amqp://localhost/",
                    new RabbitMqManagementApiOptions(new Uri("http://localhost:15672/api")),
                    o => o
                    .AddSubscriber("SerialSubscriber", s => s
                        .UseConcurrencyLimit(1)
                        .FromPublisher(PublisherInfo.Name, p =>
                            p.RegisterEventsSubscriptionsFromAssembly(typeof(PublisherInfo).Assembly)
                        )
                    )
                );
            })
            .RunConsoleAsync();
        }
    }
}
