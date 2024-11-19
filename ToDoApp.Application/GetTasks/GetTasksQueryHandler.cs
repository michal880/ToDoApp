using MediatR;
using ToDoApp.Domain;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.GetTasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IEnumerable<TaskEntity>>
{
    private readonly ITaskRepository _repository;

    public GetTasksQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskEntity>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}