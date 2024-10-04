using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;
using TodoAPI.API.Services;

namespace TodoAPI.Tests;

public static class TestsHelper
{
	public static TodoDBContext CreateDBContext()
	{
		DbContextOptions<TodoDBContext> options = new DbContextOptionsBuilder<TodoDBContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		return new TodoDBContext(options);
	}

	public static IUnitOfWork CreateUnitOfWork(TodoDBContext dbContext)
	{
		// create dependencies
		ITodoTaskRepository taskRepository = new TodoTaskRepository(dbContext);
		ITodoGoalRepository goalRepository = new TodoGoalRepository(dbContext);
		ITodoTaskService taskService = new TodoTaskService(dbContext, taskRepository);
		ITodoGoalService goalService = new TodoGoalService(goalRepository);

		return new UnitOfWork(dbContext, taskRepository, goalRepository, taskService, goalService);
	}

}
