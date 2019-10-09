using System;
using System.Threading.Tasks;
using _00_Contract;
using _06_EventBus.ParallelSubscriber.EventHandlers;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _06_EventBus.ParallelSubscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ParallelHandler>();

                services.AddRabbitMqEventBus(
                    hostContext.HostingEnvironment.EnvironmentName,
                    "amqp://localhost/",
                    new RabbitMqManagementApiOptions(new Uri("http://localhost:15672/api")),
                    o => o
                    .AddSubscriber("ParallelSubscriber", s => s
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
