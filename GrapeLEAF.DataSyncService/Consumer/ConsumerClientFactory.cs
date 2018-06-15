
namespace GrapeLEAF.DataSyncService
{
    public class ConsumerClientFactory : IConsumerClientFactory
    {
        public readonly RabbitMQOptions _rabbitMQOptions;

        public ConsumerClientFactory(RabbitMQOptions rabbitMQOptions)
        {
            _rabbitMQOptions = rabbitMQOptions;
        }

        public IConsumerClient Create(string queueName, string exchangeName = null)
        {
            return new ConsumerClient(queueName, exchangeName, _rabbitMQOptions);
        }
    }
}
