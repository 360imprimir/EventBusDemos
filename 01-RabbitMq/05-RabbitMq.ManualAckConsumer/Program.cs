using System;
using RabbitMQ.Client;
using static _00_TopologyDefinition.DemoTopology;

namespace _05_RabbitMq.ManualAckConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://localhost")
            };

            using (IConnection connection = factory.CreateConnection("Consumer-Demo-04"))
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.BasicQos(0, 10, false);
                    channel.BasicQos(0, 15, true);

                    Console.WriteLine("Consuming messages from the AppConsumer1.");
                    channel.BasicConsume(Queue.AppConsumer1, false, string.Empty, null, new MyBasicConsumer(channel));
                    Console.ReadLine();
                }
            }
        }
    }
}
