using MediatR;
using ToDoApp.Domain;

namespace ToDoApp.Application.GetTasks;

public class GetTasksQuery : IRequest<IEnumerable<TaskEntity>> { }
