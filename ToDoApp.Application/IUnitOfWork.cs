namespace ToDoApp.Application;

public interface IUnitOfWork
{ 
    Task Commit(CancellationToken cancellationToken = default);
}