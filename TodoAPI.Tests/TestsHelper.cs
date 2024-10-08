using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;
using TodoAPI.API.Services;
using TodoAPI.Data.Mappers;

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
		ITodoTaskGoalRepository taskGoalRepository = new TodoTaskGoalRepository(dbContext);

		ITodoTaskGoalService taskGoalService = new TodoTaskGoalService(taskGoalRepository);
		ITodoGoalService goalService = new TodoGoalService(goalRepository, taskGoalService);
		ITodoTaskService taskService = new TodoTaskService(dbContext, taskRepository, goalService, taskGoalService);

		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new MappingProfile());
		}));

		return new UnitOfWork(dbContext, mapper,
			taskRepository, goalRepository,
			taskService, goalService, taskGoalService);
	}

}
