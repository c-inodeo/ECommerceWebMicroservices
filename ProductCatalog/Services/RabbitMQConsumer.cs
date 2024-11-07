﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UserNotificationMessages.Helpers;
using System.Text.Json;

namespace ProductCatalog.Services
{
    public class RabbitMQConsumer : BackgroundService
    {

        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public RabbitMQConsumer(
            IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeRabbitMQ();
        }
        public void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "user_notif");

            Console.WriteLine("===> Listening on message bus");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("---> Event Received!");

                var body = ea.Body;
                Console.WriteLine($"body {body}");
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"message {message}");
                var notificationMessage = JsonSerializer.Deserialize<UserNotifModel>(message);

                Console.WriteLine($"Notification Message: Hi {notificationMessage.UserId}, {notificationMessage.Message} you are successfully registered/loggedin at {notificationMessage.TimeStamp}");                
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("----> Connection Shutdown");
        }
        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
