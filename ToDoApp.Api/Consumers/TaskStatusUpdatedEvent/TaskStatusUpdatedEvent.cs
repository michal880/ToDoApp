﻿namespace ToDoApp.Consumers.TaskStatusUpdatedEvent;
public class TaskUpdatedEvent
{
    public TaskUpdatedEvent(int id, TaskStatus oldStatus, TaskStatus newStatus, DateTime timestamp)
    {
        Id = id;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        Timestamp = timestamp;
    }

    public int Id { get; }
    public TaskStatus OldStatus { get; }
    public  TaskStatus NewStatus { get; }
    public DateTime Timestamp { get; }
}