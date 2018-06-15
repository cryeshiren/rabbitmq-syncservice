
namespace GrapeLEAF.DataSyncService
{
    public class RabbitMQOptions
    {
        internal const string ExchangeType = "direct";

        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public string TopicExchangeName { get; set; }

        public int RequestedConnectionTimeout { get; set; }

        public int SocketReadTimeout { get; set; }

        public int SocketWriteTimeout { get; set; }

        public int Port { get; set; }
    }
}
