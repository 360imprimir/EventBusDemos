using System.Collections.Generic;
using System.Text;
using _00_TopologyDefinition;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace _01_RabbitMq.Producer
{
    internal static class Messages
    {
        public static Message CreateMessage(IModel channel)
        {
            byte[] body = Encoding.UTF8.GetBytes("Hello world!");
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";

            return new Message
            {
                Body = body,
                Properties = properties
            };
        }

        public static Message CreateTextMessage(IModel channel)
        {
            byte[] body = Encoding.UTF8.GetBytes("Hello world!");
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";
            properties.Headers = new Dictionary<string, object> { { "format", "text" } };

            return new Message
            {
                Body = body,
                Properties = properties
            };
        }

        public static Message CreateJsonMessage(IModel channel)
        {
            byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("Hello world!"));
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Headers = new Dictionary<string, object> { { "format", "json" } };

            return new Message
            {
                Body = body,
                Properties = properties
            };
        }
    }
}
