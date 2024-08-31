using TodoAPI.Repositories;

namespace TodoAPI.Services;

public interface IUnitOfWork : IDisposable
{
    public ITodoTaskRepository TaskRepository { get; }
    public ITodoGoalRepository GoalRepository { get; }
    public Task<int> Save();
}
