using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Infrastructure;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _dbContext;

    public TaskRepository(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TaskEntity? task)
    {
        await _dbContext.Tasks.AddAsync(task);
    }

    public async Task<TaskEntity?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FindAsync(id);
    }

    public async Task<IEnumerable<TaskEntity?>> GetAllAsync()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    public Task UpdateAsync(TaskEntity? task)
    { 
        _dbContext.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}