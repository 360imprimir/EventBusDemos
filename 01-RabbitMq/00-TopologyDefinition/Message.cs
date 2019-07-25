using RabbitMQ.Client;

namespace _00_TopologyDefinition
{
    public class Message
    {
        public byte[] Body { get; set; }
        public IBasicProperties Properties { get; set; }
    }
}
