using System;
using System.Text;
using RabbitMQ.Client;

namespace _05_RabbitMq.ManualAckConsumer
{
    class MyBasicConsumer : DefaultBasicConsumer
    {
        public MyBasicConsumer(IModel model) : base(model)
        {
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine($"Received message number {deliveryTag} with the body: {Encoding.UTF8.GetString(body)}");

            if (deliveryTag % 2 == 0)
            {
                Console.WriteLine($"Ack message number {deliveryTag}.");
                Model.BasicAck(deliveryTag, false);
            } else
            {
                Console.WriteLine($"Nack message number {deliveryTag}.");
                Model.BasicNack(deliveryTag, false, false);
            }
        }
    }
}
