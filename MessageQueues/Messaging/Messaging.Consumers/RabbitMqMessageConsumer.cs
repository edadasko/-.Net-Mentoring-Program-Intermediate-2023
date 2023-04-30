using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Consumers
{
    public class RabbitMqMessageConsumer : IMessageConsumer, IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly EventingBasicConsumer consumer;

        private const string ExchangeName = "testExchange";
        private const string QueueName = "testQueue";
        private const string ConnectionUri = "amqp://guest:guest@localhost:5672";

        public RabbitMqMessageConsumer(Func<byte[], Task> processAction)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(ConnectionUri);

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(QueueName, true, false, false);
            channel.QueueBind(QueueName, ExchangeName, "");

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                await processAction(eventArgs.Body.ToArray());
            };
        }

        public void Listen(EventWaitHandle waitingHandle)
        {
            channel.BasicConsume(QueueName, true, consumer);
            waitingHandle.WaitOne();
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
