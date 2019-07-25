namespace _00_TopologyDefinition
{
    public static class DemoTopology
    {
        public static class Exchange
        {
            public const string Direct = "Demo.Direct.Exchange";
            public const string Fanout = "Demo.Fanout.Exchange";
            public const string Topic = "Demo.Topic.Exchange";
            public const string Headers = "Demo.Headers.Exchange";
        }

        public static class Queue
        {
            public const string AppConsumer1 = "Demo.AppConsumer1.Queue";
            public const string AppConsumer2 = "Demo.AppConsumer2.Queue";
            public const string ErrorLoggingConsumer = "Demo.ErrorLoggingConsumer.Queue";
            public const string JsonConsumer = "Demo.JsonConsumer.Queue";
            public const string TextConsumer = "Demo.TextConsumer.Queue";
        }

        public static class RoutingKey
        {
            public const string AppConsumer1 = "AppConsumer1";
            public const string AppConsumer2 = "AppConsumer2";
        }
    }
}
