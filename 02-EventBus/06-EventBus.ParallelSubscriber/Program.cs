using _00_Contract;
using _06_EventBus.ParallelSubscriber.EventHandlers;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace _06_EventBus.ParallelSubscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .CreateLogger();

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
                            p.RegisterEventSubscriptionsFromAssembly(typeof(PublisherInfo).Assembly)
                        )
                    )
                );
            })
            .RunConsoleAsync();
        }
    }
}
