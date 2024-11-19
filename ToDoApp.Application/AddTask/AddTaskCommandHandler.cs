using MediatR;
using ToDoApp.Domain;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.AddTask;

public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, int>
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTaskCommandHandler(ITaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskEntity(request.ID, request.Name, request.Description, request.AssignedTo);
        await _repository.AddAsync(task);
        await _unitOfWork.Commit(cancellationToken);
        return task.ID;
    }
}