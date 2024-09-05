using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public interface IUnitOfWork : IDisposable
{
    public ITodoTaskRepository TaskRepository { get; }
    public ITodoGoalRepository GoalRepository { get; }
    public Task<int> Save();
}
