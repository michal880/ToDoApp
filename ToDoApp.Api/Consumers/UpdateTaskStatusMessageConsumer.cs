using MediatR;
using ToDoApp.Application.UpdateTaskStatus;

namespace ToDoApp.Consumers;

public class UpdateTaskStatusMessageConsumer : IConsumer<UpdateTaskStatusMessage>
{
    private readonly IMediator _mediator;

    public UpdateTaskStatusMessageConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task ConsumeAsync(UpdateTaskStatusMessage message)
    {
        var cmd = new UpdateTaskStatusCommand()
        {
            Id = message.Id,
            NewStatus = message.NewStatus
        };
        await _mediator.Publish(cmd);
    }
}