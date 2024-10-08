
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public class UnitOfWork : IUnitOfWork
{
	readonly TodoDBContext _dbContext;

	public IMapper Mapper { get; }

	// repositories
	public ITodoTaskRepository TaskRepository { get; }
	public ITodoGoalRepository GoalRepository { get; }

	// services
	public ITodoTaskService TaskService { get; }
	public ITodoGoalService GoalService { get; }

	public ITodoTaskGoalService TaskGoalService { get; }

	public UnitOfWork(TodoDBContext dbContext, IMapper mapper,
		ITodoTaskRepository taskRepository, ITodoGoalRepository goalRepository,
		ITodoTaskService taskService, ITodoGoalService goalService, ITodoTaskGoalService taskGoalService)
	{
		_dbContext = dbContext;
		Mapper = mapper;

		TaskRepository = taskRepository;
		GoalRepository = goalRepository;

		TaskService = taskService;
		GoalService = goalService;
		TaskGoalService = taskGoalService;
	}

	public async Task<int> Save()
	{
		try
		{
			return await _dbContext.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			Console.WriteLine("DbUpdateConcurrencyException");
		}
		return 0;
	}


	public void Dispose()
		=> _dbContext.Dispose();

}
