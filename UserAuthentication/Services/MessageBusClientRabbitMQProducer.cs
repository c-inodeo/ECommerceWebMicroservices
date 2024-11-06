using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace UserAuthentication.Services
{
    public class MessageBusClientRabbitMQProducer : IMessageBusClientRabbitMQProducer, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClientRabbitMQProducer(IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("---> Connected to Message Bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"----> Could not connect to the Message Bus : {ex.Message}");
            }
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($"--> Sent message: {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Disposing MessageBusClient");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
        }
    }
}
