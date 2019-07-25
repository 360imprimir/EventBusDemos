using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using static _00_TopologyDefinition.DemoTopology;

namespace _01_RabbitMq.Topology
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://localhost")
            };

            using (IConnection connection = factory.CreateConnection("Topology-Demo-01"))
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(Exchange.Direct, ExchangeType.Direct, true, false, null);
                    channel.ExchangeDeclare(Exchange.Fanout, ExchangeType.Fanout, true, false, null);
                    channel.ExchangeDeclare(Exchange.Topic, ExchangeType.Topic, true, false, null);
                    channel.ExchangeDeclare(Exchange.Headers, ExchangeType.Headers, true, false, null);

                    channel.QueueDeclare(Queue.AppConsumer1, true, false, false, null);
                    channel.QueueDeclare(Queue.AppConsumer2, true, false, false, null);
                    channel.QueueDeclare(Queue.ErrorLoggingConsumer, true, false, false, null);
                    channel.QueueDeclare(Queue.JsonConsumer, true, false, false, null);
                    channel.QueueDeclare(Queue.TextConsumer, true, false, false, null);

                    channel.QueueBind(Queue.AppConsumer1, Exchange.Direct, RoutingKey.AppConsumer1, null);
                    channel.QueueBind(Queue.AppConsumer2, Exchange.Direct, RoutingKey.AppConsumer2, null);

                    channel.QueueBind(Queue.AppConsumer1, Exchange.Fanout, string.Empty, null);
                    channel.QueueBind(Queue.AppConsumer2, Exchange.Fanout, string.Empty, null);

                    channel.QueueBind(Queue.AppConsumer1, Exchange.Topic, $"{RoutingKey.AppConsumer1}.*", null);
                    channel.QueueBind(Queue.AppConsumer2, Exchange.Topic, $"{RoutingKey.AppConsumer2}.*", null);
                    channel.QueueBind(Queue.ErrorLoggingConsumer, Exchange.Topic, "*.Error", null);

                    channel.QueueBind(Queue.JsonConsumer, Exchange.Headers, string.Empty, new Dictionary<string, object> { { "format", "json" } });
                    channel.QueueBind(Queue.TextConsumer, Exchange.Headers, string.Empty, new Dictionary<string, object> { { "format", "text" } });
                }
            }
        }
    }
}
