namespace ToDoApp.Domain.Repositories;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TaskEntity?>> GetAllAsync();
    Task AddAsync(TaskEntity task);
    Task UpdateAsync(TaskEntity task);
}