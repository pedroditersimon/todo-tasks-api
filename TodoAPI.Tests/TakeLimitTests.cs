using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

public class TakeLimitTests
{
	[Fact]
	public async void GetAllTasks_WithLimit()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		using IUnitOfWork unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// create some tasks
		for (int i = 0; i < 4; i++)
		{
			unitOfWork.TaskService.Create(new TodoTask() { ID = i + 1 });
		}
		await unitOfWork.Save();

		// get only 2
		List<TodoTask> tasks = await unitOfWork.TaskService.GetAll(limit: 2).ToListAsync();

		Assert.Equal(2, tasks.Count);
		Assert.NotNull(tasks[0]);
	}

	[Fact]
	public async void GetAllTasks_WithoutLimit()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		using IUnitOfWork unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// create some tasks
		for (int i = 0; i < 4; i++)
		{
			await unitOfWork.TaskService.Create(new TodoTask() { ID = i + 1 });
		}
		await unitOfWork.Save();

		// get only 2
		List<TodoTask> tasks = await unitOfWork.TaskService.GetAll(limit: 0).ToListAsync();

		Assert.Equal(4, tasks.Count);
		Assert.NotNull(tasks[0]);
	}
}
