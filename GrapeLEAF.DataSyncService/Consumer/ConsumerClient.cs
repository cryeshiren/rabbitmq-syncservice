using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeLEAF.DataSyncService
{
    public class ConsumerClient : IConsumerClient
    {
        private readonly RabbitMQOptions _rabbitMQOptions;

        private readonly string _exchageName;
        private readonly string _queueName;
        private ulong _deliveryTag;

        private IConnection _connection;
        private IModel _channel;

        public event EventHandler<Message> MessageReceieved;

        public ConsumerClient(string queueName, string exchangeName, RabbitMQOptions options)
        {
            _queueName = queueName;
            _exchageName = exchangeName;
            _rabbitMQOptions = options;

            InitClient();
        }

        private void InitClient()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMQOptions.HostName,
                UserName = _rabbitMQOptions.UserName,
                Port = _rabbitMQOptions.Port,
                Password = _rabbitMQOptions.Password,
                VirtualHost = _rabbitMQOptions.VirtualHost,
                RequestedConnectionTimeout = _rabbitMQOptions.RequestedConnectionTimeout,
                SocketReadTimeout = _rabbitMQOptions.SocketReadTimeout,
                SocketWriteTimeout = _rabbitMQOptions.SocketWriteTimeout,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = 60
            };

            _connection = connectionFactory.CreateConnection();

            _channel = CreateModel();

            _channel.ExchangeDeclare(exchange: _exchageName ?? _rabbitMQOptions.TopicExchangeName, type: RabbitMQOptions.ExchangeType, durable: true);

            _channel.QueueDeclare(_queueName, exclusive: false, durable: true, autoDelete: false);
        }

        public void Commit()
        {
            _channel.BasicAck(_deliveryTag, false);
        }

        public void Reject(bool requeue)
        {
            _channel.BasicReject(_deliveryTag, requeue);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnConsumerReceived;

            _channel.BasicConsume(_queueName, false, consumer);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Task.Delay(timeout, cancellationToken).Wait();
                }
                catch (Exception e){}
            }
        }

         public void Subscribe(string topic)
        {
            _channel.QueueBind(_queueName, _rabbitMQOptions.TopicExchangeName, topic);
        }

        public void Subscribe(string topic, string group)
        {
            _channel.QueueBind(_queueName, topic ?? _rabbitMQOptions.TopicExchangeName, group);
        }

        public void Subscribe(string topic, int partition)
        {
            _channel.QueueBind(_queueName, _rabbitMQOptions.TopicExchangeName, topic);
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            _deliveryTag = e.DeliveryTag;

            var message = new Message
            {
                Group = e.RoutingKey,
                Name = _queueName,
                Content = Encoding.UTF8.GetString(e.Body)
            };

            MessageReceieved?.Invoke(sender, message);
        }

        private bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        private IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

    }
}
