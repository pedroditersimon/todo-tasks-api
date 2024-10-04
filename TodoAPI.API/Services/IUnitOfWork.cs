using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public interface IUnitOfWork : IDisposable
{
	// repositories
	public ITodoTaskRepository TaskRepository { get; }
	public ITodoGoalRepository GoalRepository { get; }

	// services
	public ITodoTaskService TaskService { get; }
	public ITodoGoalService GoalService { get; }

	public Task<int> Save();
}
