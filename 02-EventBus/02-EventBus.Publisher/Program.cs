using System;
using System.Threading;
using System.Threading.Tasks;
using _00_Contract;
using BinarySubject.Library.EventBus.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions;
using BinarySubject.Library.EventBus.Configuration.Abstractions.Builder;
using BinarySubject.Library.EventBus.RabbitMq.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace _02_EventBus.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .CreateLogger();

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                    services.AddRabbitMqEventBus(
                        hostContext.HostingEnvironment.EnvironmentName,
                        "amqp://localhost/",
                        new RabbitMqManagementApiOptions(new Uri("http://localhost:15672/api")),
                        o =>
                        o.AddPublisher(PublisherInfo.Name, p =>
                            p.RegisterEvent<OrderCancelled>()
                        )
                    )
                ).Build();

            var eventPublisher = host.Services.GetService<IEventPublisher>();

            for (int i = 0; i < 10; ++i)
            {
                try
                {
                    await eventPublisher.Publish(new OrderCancelled(), CancellationToken.None);
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "Error publishing event.");
                }
            }

            await host.RunAsync();
        }
    }
}
