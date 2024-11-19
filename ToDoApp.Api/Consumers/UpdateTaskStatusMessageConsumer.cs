using MassTransit;
using MediatR;
using ToDoApp.Application.UpdateTaskStatus;

namespace ToDoApp;

public class UpdateTaskStatusMessageConsumer : IConsumer<UpdateTaskStatusMessage>
{
    private readonly IMediator _mediator;

    public UpdateTaskStatusMessageConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<UpdateTaskStatusMessage> context)
    {
        var cmd = new UpdateTaskStatusCommand()
        {
            Id = context.Message.Id,
            NewStatus = context.Message.NewStatus
        };
        await _mediator.Publish(cmd);
    }
}
public class TaskUpdatedEventConsumer : IConsumer<TaskUpdatedEvent>
{
    public async Task Consume(ConsumeContext<TaskUpdatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine($"Task Updated: {message.Id}, New Status: {message.NewStatus}");
        await Task.CompletedTask;
    }
}