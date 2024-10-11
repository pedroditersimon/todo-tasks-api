using TodoAPI.API.Repositories;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;
public class TaskRepositoryTests
{

	[Fact]
	public async void GetByID()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		TodoTask? task = await taskRepository.GetByID(1);
		Assert.NotNull(task);
	}


	[Fact]
	public async void GetByID_NotFound()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		TodoTask? task = await taskRepository.GetByID(2);
		Assert.Null(task);
	}


	[Fact]
	public async void GetByID_NotFound_WhenTaskIsHardDeletedWithoutSaving()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		await taskRepository.HardDelete(1);

		TodoTask? task = await taskRepository.GetByID(2);
		Assert.Null(task);
	}

	[Fact]
	public async void GetByID_NotFound_WhenTaskIsSoftDeletedWithoutSaving()
	{
		using TodoDBContext dbContext = TestsHelper.CreateDBContext();
		TodoTaskRepository taskRepository = new(dbContext);

		await taskRepository.Create(new TodoTask() { ID = 1 });
		await dbContext.SaveChangesAsync();

		await taskRepository.SoftDelete(1);

		TodoTask? task = await taskRepository.GetByID(2);
		Assert.Null(task);
	}
}