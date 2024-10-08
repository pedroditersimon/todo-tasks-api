using AutoMapper;
using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public interface IUnitOfWork : IDisposable
{
	public IMapper Mapper { get; }

	// repositories
	public ITodoTaskRepository TaskRepository { get; }
	public ITodoGoalRepository GoalRepository { get; }

	// services
	public ITodoTaskService TaskService { get; }
	public ITodoGoalService GoalService { get; }

	public ITodoTaskGoalService TaskGoalService { get; }

	public Task<int> Save();
}
