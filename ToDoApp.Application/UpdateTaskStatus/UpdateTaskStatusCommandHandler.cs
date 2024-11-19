using MediatR;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;

    public UpdateTaskStatusCommandHandler(ITaskRepository repository, IUnitOfWork unitOfWork, IServiceBus serviceBus)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
    }

    public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id);
        if (task == null)
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");

        var @event = new TaskUpdatedEvent(task.ID, request.NewStatus, task.Status, DateTime.Now);
        task.UpdateStatus(request.NewStatus);
        await _repository.UpdateAsync(task);
        await _unitOfWork.Commit(cancellationToken);
        await _serviceBus.PublishAsync(@event,cancellationToken);
    }
}