using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ToDoApp.ServiceBus;

public class RabbitMqPublisher
{
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> rabbitSettings, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;
        var settingsValues = rabbitSettings.Value;
        var factory = new ConnectionFactory
        {
            HostName = settingsValues.Host,
            UserName = settingsValues.Username,
            Password = settingsValues.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
    {
        try
        {
            var queueName = typeof(T).Name;
            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
           
            var serializedMessage = JsonSerializer.Serialize(message);

            var messageEnvelope = new MessageEnvelope
            {
                Type = queueName,
                Data = serializedMessage
            };

            var jsonMessage = JsonSerializer.Serialize(messageEnvelope);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            
            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body);

            _logger.LogInformation($"Message sent to queue: {queueName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending message: {ex.Message}");
            throw;
        }

        return Task.CompletedTask;
    }
}