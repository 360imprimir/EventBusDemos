using System;
using System.Text;
using RabbitMQ.Client;
using static _00_TopologyDefinition.DemoTopology;

namespace _03_RabbitMq.ConfirmsAwareProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://localhost")
            };

            using (IConnection connection = factory.CreateConnection("Producer-Demo-03"))
            {
                using (IModel channel = connection.CreateModel())
                {
                    byte[] body = Encoding.UTF8.GetBytes("Hello world!");
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.ContentType = "text/plain";

                    channel.ConfirmSelect();

                    Console.WriteLine("Publishing message to the Direct Exchange with the AppConsumer1 routing key.");
                    channel.BasicPublish(Exchange.Direct, RoutingKey.AppConsumer1, false, properties, body);
                    Console.WriteLine("Waiting for message to be confirmed.");
                    var ack = channel.WaitForConfirms();
                    if(ack)
                    {
                        Console.WriteLine("Message confirmed.");
                    } else
                    {
                        Console.WriteLine("Message not confirmed.");
                    }
                    
                    Console.ReadLine();

                    channel.BasicAcks += Channel_BasicAcks;
                    channel.BasicNacks += Channel_BasicNacks;

                    Console.WriteLine("Publishing message to the Direct Exchange with the AppConsumer1 routing key.");
                    var messageNumber = channel.NextPublishSeqNo;
                    channel.BasicPublish(Exchange.Direct, RoutingKey.AppConsumer1, false, properties, body);
                    Console.WriteLine($"Waiting for message {messageNumber} to be confirmed.");
                    Console.ReadLine();
                }
            }
        }

        private static void Channel_BasicNacks(object sender, RabbitMQ.Client.Events.BasicNackEventArgs e)
        {
            Console.WriteLine($"Message number {e.DeliveryTag} not confirmed.");
        }

        private static void Channel_BasicAcks(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
        {
            Console.WriteLine($"Message number {e.DeliveryTag} confirmed.");
        }
    }
}
