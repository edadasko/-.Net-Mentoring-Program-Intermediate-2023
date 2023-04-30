using RabbitMQ.Client;

namespace Messaging.Producers
{
    public class RabbitMqMessageProducer : IMessageProducer, IDisposable
    {
        private const string ExchangeName = "testExchange";
        private const string ConnectionUri = "amqp://guest:guest@localhost:5672";

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqMessageProducer()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(ConnectionUri);

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, true);
        }

        public void SendMessage(byte[] message)
        {
            _channel.BasicPublish(ExchangeName, "", null, message);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
