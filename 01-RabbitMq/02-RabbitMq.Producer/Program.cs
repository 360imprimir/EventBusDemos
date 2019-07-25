using System;
using RabbitMQ.Client;
using static _00_TopologyDefinition.DemoTopology;

namespace _01_RabbitMq.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://localhost")
            };

            using (IConnection connection = factory.CreateConnection("Producer-Demo-02"))
            {
                using (IModel channel = connection.CreateModel())
                {
                    var message = Messages.CreateMessage(channel);
                    Console.WriteLine("Publishing message to the Direct Exchange with the AppConsumer1 routing key.");
                    channel.BasicPublish(Exchange.Direct, RoutingKey.AppConsumer1, false, message.Properties, message.Body);
                    Console.ReadLine();
                    Console.WriteLine("Publishing message to the Direct Exchange with the AppConsumer2 routing key.");
                    channel.BasicPublish(Exchange.Direct, RoutingKey.AppConsumer2, false, message.Properties, message.Body);
                    Console.ReadLine();

                    Console.WriteLine("Publishing message to the Fanout Exchange.");
                    channel.BasicPublish(Exchange.Fanout, string.Empty, false, message.Properties, message.Body);
                    Console.ReadLine();

                    Console.WriteLine("Publishing message to the Topic Exchange with the AppConsumer1.Info routing key.");
                    channel.BasicPublish(Exchange.Topic, $"{RoutingKey.AppConsumer1}.Info", false, message.Properties, message.Body);
                    Console.ReadLine();
                    Console.WriteLine("Publishing message to the Topic Exchange with the AppConsumer2.Info routing key.");
                    channel.BasicPublish(Exchange.Topic, $"{RoutingKey.AppConsumer2}.Info", false, message.Properties, message.Body);
                    Console.ReadLine();
                    Console.WriteLine("Publishing message to the Topic Exchange with the AppConsumer1.Error routing key.");
                    channel.BasicPublish(Exchange.Topic, $"{RoutingKey.AppConsumer1}.Error", false, message.Properties, message.Body);
                    Console.ReadLine();

                    var textMessage = Messages.CreateTextMessage(channel);
                    Console.WriteLine("Publishing message to the Headers Exchange with the text format header.");
                    channel.BasicPublish(Exchange.Headers, string.Empty, false, textMessage.Properties, textMessage.Body);
                    Console.ReadLine();

                    var jsonMessage = Messages.CreateJsonMessage(channel);
                    Console.WriteLine("Publishing message to the Headers Exchange with the json format header.");
                    channel.BasicPublish(Exchange.Headers, string.Empty, false, jsonMessage.Properties, jsonMessage.Body);
                    Console.ReadLine();
                }
            }
        }
    }
}
