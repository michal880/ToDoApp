﻿namespace ToDoApp.Domain;

public class TaskEntity
{
    public TaskEntity() { }
    public int ID { get;  set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public string? AssignedTo { get; private set; }

    public TaskEntity(int id, string name, string description, string? assignedTo = null)
    {
        ID = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Status = TaskStatus.NotStarted;
        AssignedTo = assignedTo;
    }

    public void UpdateStatus(TaskStatus newStatus)
    {
        if (newStatus == Status)
        {
            throw new InvalidOperationException("Task is already in the specified status.");
        }

        Status = newStatus;
    }
}