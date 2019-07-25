using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static _00_TopologyDefinition.DemoTopology;

namespace _04_RabbitMq.Consumer
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
                    var appConsumer1Tag = channel.BasicConsume(Queue.AppConsumer1, true, string.Empty, null, new MyBasicConsumer(channel));
                    Console.ReadLine();
                    channel.BasicCancel(appConsumer1Tag);

                    var eventingConsumer = new EventingBasicConsumer(channel);
                    eventingConsumer.Received += EventingConsumer_Received;

                    Console.WriteLine("Consuming messages from the AppConsumer2.");
                    var appConsumer2Tag = channel.BasicConsume(Queue.AppConsumer2, true, string.Empty, null, eventingConsumer);
                    Console.ReadLine();
                    channel.BasicCancel(appConsumer2Tag);
                }
            }
        }

        private static void EventingConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"Received message number {e.DeliveryTag} with the body: {Encoding.UTF8.GetString(e.Body)}");
        }
    }
}
