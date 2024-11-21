namespace ToDoApp.Consumers;

public class UpdateTaskStatusMessage
{
    public int Id { get; set; }
    public  TaskStatus NewStatus { get; set; }
}