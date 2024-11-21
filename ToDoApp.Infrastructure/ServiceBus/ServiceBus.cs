using ToDoApp.Application;
using ToDoApp.ServiceBus;

namespace ToDoApp.Infrastructure.ServiceBus;

public class ServiceBus : IServiceBus
{
    private readonly RabbitMqPublisher _publishEndpoint;

    public ServiceBus(RabbitMqPublisher publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
    {
        return _publishEndpoint.PublishAsync(message, cancellationToken);
    }
}