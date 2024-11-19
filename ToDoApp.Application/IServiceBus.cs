namespace ToDoApp.Application;

public interface IServiceBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
}