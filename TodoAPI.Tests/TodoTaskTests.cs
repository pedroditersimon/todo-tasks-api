using TodoAPI.API.Repositories;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;
public class TodoTaskTests
{

	[Fact]
	public async void GetByID()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		TodoTask? task = await taskRepository.GetByID(1);
		Assert.True(task != null);
	}


	[Fact]
	public async void GetByID_NotFound()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		TodoTask? task = await taskRepository.GetByID(2);
		Assert.True(task == null);
	}
}