using System;
using System.Text;
using RabbitMQ.Client;

namespace _04_RabbitMq.Consumer
{
    class MyBasicConsumer : DefaultBasicConsumer
    {
        public MyBasicConsumer(IModel model) : base(model)
        {
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine($"Received message number {deliveryTag} with the body: {Encoding.UTF8.GetString(body)}");
        }
    }
}
