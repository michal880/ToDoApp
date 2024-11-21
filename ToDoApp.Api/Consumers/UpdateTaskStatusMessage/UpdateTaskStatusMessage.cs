namespace ToDoApp.Consumers.UpdateTaskStatusMessage;

public class UpdateTaskStatusMessage
{
    public int Id { get; set; }
    public  TaskStatus NewStatus { get; set; }
}