using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.AddTask;
using ToDoApp.Application.GetTasks;
using ToDoApp.Application.UpdateTaskStatus;


namespace ToDoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] AddTaskCommand task)
    {
        var taskId = await _mediator.Send(task);
        return Created( $"/api/tasks/{taskId}", new { Id = taskId });
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _mediator.Send(new GetTasksQuery());
        return Ok(tasks);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatus newStatus)
    {
        await _mediator.Send(new UpdateTaskStatusCommand { Id = id, NewStatus = newStatus });
        return NoContent();
    }
}