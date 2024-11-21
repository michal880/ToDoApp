namespace ToDoApp.Consumers;

public class TaskStatusUpdatedEventConsumer : IConsumer<TaskUpdatedEvent>
{
    public async Task ConsumeAsync(TaskUpdatedEvent message)
    {
       Console.WriteLine($"Processed event {message.GetType()}");
    }
}
