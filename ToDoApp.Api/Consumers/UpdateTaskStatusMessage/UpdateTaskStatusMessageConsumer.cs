using MediatR;
using ToDoApp.Application.UpdateTaskStatus;
using ToDoApp.ServiceBus;

namespace ToDoApp.Consumers.UpdateTaskStatusMessage;

public class UpdateTaskStatusMessageConsumer : IConsumer<UpdateTaskStatusMessage>
{
    private readonly IMediator _mediator;

    public UpdateTaskStatusMessageConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task ConsumeAsync(Consumers.UpdateTaskStatusMessage.UpdateTaskStatusMessage message)
    {
        var cmd = new UpdateTaskStatusCommand()
        {
            Id = message.Id,
            NewStatus = message.NewStatus
        };
        await _mediator.Publish(cmd);
    }
}