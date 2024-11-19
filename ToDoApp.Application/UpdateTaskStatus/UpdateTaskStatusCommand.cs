using MediatR;

namespace ToDoApp.Application.UpdateTaskStatus;
public class UpdateTaskStatusCommand : IRequest
{
    public int Id { get; set; }
    public  ToDoApp.Domain.TaskStatus NewStatus { get; set; }
}