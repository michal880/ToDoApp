using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ToDoApp.Infrastructure;

namespace ToDoApp;

public class RabbitMqListenerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqListenerService> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly List<Type> _knownMessageTypes;

    public RabbitMqListenerService(IOptions<RabbitMqSettings> rabbitSettings, IServiceProvider serviceProvider,
        ILogger<RabbitMqListenerService> logger)
    {
        var settingsValues = rabbitSettings.Value;
        var factory = new ConnectionFactory
        {
            HostName = settingsValues.Host,
            UserName = settingsValues.Username,
            Password = settingsValues.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _knownMessageTypes = new List<Type>();

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerTypes = GetAllConsumerTypes();

        foreach (var consumerType in consumerTypes)
        {
            var consumerInterface = consumerType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));

            var messageType = consumerInterface?.GetGenericArguments().FirstOrDefault();
            if (messageType != null)
            {
                _knownMessageTypes.Add(messageType);
                var queueName = messageType.Name;
                DeclareQueue(queueName);
                ListenToQueue(queueName);
            }
        }

        return Task.CompletedTask;
    }

    private void DeclareQueue(string queueName)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    private void ListenToQueue(string queueName)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            try
            {
                var envelope = JsonSerializer.Deserialize<MessageEnvelope>(messageJson);
                if (envelope == null || string.IsNullOrEmpty(envelope.Type))
                    throw new InvalidOperationException("Invalid message envelope.");

                var messageType = _knownMessageTypes.First(t => t.Name == envelope.Type);
                if (messageType == null)
                    throw new InvalidOperationException($"Unknown message type: {envelope.Type}");

                var message = JsonSerializer.Deserialize(envelope.Data, messageType);
                if (message == null)
                    throw new InvalidOperationException("Message deserialization failed.");

                await ResolveConsumer(messageType, message);

                _channel.BasicAck(ea.DeliveryTag, false);
                _logger.LogInformation($"Message processed from queue '{queueName}': {messageJson}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error processing message from queue '{queueName}': {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };
        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        _logger.LogInformation($"Started listening to queue '{queueName}'.");
    }

    private Task ResolveConsumer(Type messageType, object message)
    {
        using var scope = _serviceProvider.CreateScope();
        var consumerType = typeof(IConsumer<>).MakeGenericType(messageType);
        var consumerInstance = scope.ServiceProvider.GetRequiredService(consumerType);

        if (consumerInstance == null)
            throw new InvalidOperationException($"No consumer found for message type {messageType}");

        var handlerMethod = consumerType.GetMethod("ConsumeAsync", new Type[] { messageType });
        if (handlerMethod == null)
            throw new InvalidOperationException($"Method ConsumeAsync not found in {consumerType}");
        return (Task)handlerMethod.Invoke(consumerInstance, new object[] { message })!;
    }

    private static IEnumerable<Type> GetAllConsumerTypes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var consumerInterface = typeof(IConsumer<>);

        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == consumerInterface))
            .ToList();
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}