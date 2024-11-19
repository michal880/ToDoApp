using MediatR;

namespace ToDoApp.Application.AddTask;

public class AddTaskCommand : IRequest<int>
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? AssignedTo { get; set; }
}