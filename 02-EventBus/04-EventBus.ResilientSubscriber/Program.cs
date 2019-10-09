using System;
using System.Threading.Tasks;
using _00_Contract;
using _04_EventBus.ResilientSubscriber.EventHandlers;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;

namespace _04_EventBus.ResilientSubscriber
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
                services.AddSingleton<RetryHandler>();

                services.AddRabbitMqEventBus(
                    hostContext.HostingEnvironment.EnvironmentName,
                    "amqp://localhost/",
                    new RabbitMqManagementApiOptions(new Uri("http://localhost:15672/api")),
                    o => o
                    .AddSubscriber("ResilientSubscriber", s => s
                        .ConfigurePipeline(pb => pb
                            .UseResiliencePolicy(Policy.Handle<Exception>()
                                .WaitAndRetryAsync(2, ct => TimeSpan.FromSeconds(5))
                            )
                        )
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
