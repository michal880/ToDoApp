using MassTransit;
using ToDoApp.Application;

namespace ToDoApp.Infrastructure;

public class MassTransitServiceBus : IServiceBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitServiceBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
}