namespace ToDoApp.ServiceBus;

public interface IConsumer<T> where T : class
{
    public Task ConsumeAsync(T message);
}
