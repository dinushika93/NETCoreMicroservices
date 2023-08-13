using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"])};
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

                Console.WriteLine($"Succesully connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to the message bus: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"Closing the connection...");
        }


        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_channel.IsOpen)
            {
                SendMessage(message);
            }
            else
                Console.WriteLine("Could not open the connection...");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"{message} is sent...");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus is disposed...");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

        }
    }
}
