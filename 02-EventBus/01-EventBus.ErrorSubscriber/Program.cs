using _00_Contract;
using _01_EventBus.ErrorSubscriber.EventHandlers;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace _01_EventBus.ErrorSubscriber
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
                services.AddSingleton<ErrorHandler>();

                services.AddRabbitMqEventBus(
                    hostContext.HostingEnvironment.EnvironmentName,
                    "amqp://localhost/",
                    new RabbitMqManagementApiOptions(new Uri("http://localhost:15672/api")),
                    o => o
                    .AddSubscriber("ErrorSubscriber", s => s
                        .ConfigurePipeline(pb => pb
                            .UseRabbitMqRetry(1)
                        )
                        .UseConcurrencyLimit(1)
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
