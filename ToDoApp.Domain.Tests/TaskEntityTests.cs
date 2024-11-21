using System;
using Xunit;

namespace ToDoApp.Domain.Tests;

public class TaskEntityTests
{
    [Fact]
    public void UpdateTaskStatus_ShouldChangeStatus()
    {
        // Arrange
        var existingTask = new TaskEntity
            (1, "Sample Task", "Task Description", TaskStatus.NotStarted, "Assignee");
        // Act
        existingTask.UpdateStatus(TaskStatus.InProgress);

        // Assert
        Assert.Equal(TaskStatus.InProgress, existingTask.Status);
    }
    [Fact]
    public void UpdateTaskStatus_ShouldThrowWhenNewStatusIsTheSame()
    {
        // Arrange
        var existingTask = new TaskEntity(1, "Sample Task", "Task Description", TaskStatus.NotStarted, "Assignee");
        
        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => existingTask.UpdateStatus(TaskStatus.NotStarted));
    }
}