using TodoAPI.Data.DTOs.TodoTask;
using TodoAPI.Data.Mappers;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

public class MapperTests
{

	[Fact]
	public void CreateTaskRequest_ToTask()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		var createTaskRequest = new CreateTaskRequest
		{
			Name = "task name",
			Description = "task description",
			IsCompleted = true,
			IsFavorite = true
		};

		TodoTask task = createTaskRequest.ToTask(unitOfWork.Mapper);

		Assert.NotNull(task);
		Assert.Equal(createTaskRequest.Name, task.Name);
		Assert.Equal(createTaskRequest.Description, task.Description);
		Assert.Equal(createTaskRequest.IsCompleted, task.IsCompleted);
		Assert.Equal(createTaskRequest.IsFavorite, task.IsFavorite);
	}

	[Fact]
	public void Task_ToTaskResponse()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		var task = new TodoTask
		{
			ID = 1,
			Name = "task name",
			Description = "task description",
			IsCompleted = true,
			CreationDate = DateTime.UtcNow
		};

		TaskResponse taskResponse = task.ToResponse(unitOfWork.Mapper);

		Assert.NotNull(taskResponse);
		Assert.Equal(task.ID, taskResponse.ID);
		Assert.Equal(task.Name, taskResponse.Name);
		Assert.Equal(task.Description, taskResponse.Description);
		Assert.Equal(task.IsCompleted, taskResponse.IsCompleted);
		Assert.Equal(task.CreationDate, taskResponse.CreationDate);
	}


	[Fact]
	public void ReplaceWith_UpdatesProperties_Correctly()
	{
		// Arrange
		TodoTask originalTask = new()
		{
			ID = 1,
			Name = "task name",
			Description = "task description",
			IsCompleted = true,
			CreationDate = DateTime.UtcNow,
			IsFavorite = false,
			LastDeletedTime = DateTime.UtcNow,
			LastUpdatedTime = DateTime.UtcNow,
			IsDeleted = false
		};

		UpdateTaskRequest updateTask = new()
		{
			ID = 1,
			Name = "new name",
			Description = "",   // Description is empty, it should change anyway
			IsCompleted = false,
			IsFavorite = true
		};

		// Act
		TodoTask replacedTask = originalTask.ReplaceWith(updateTask);

		// Assert
		Assert.NotNull(replacedTask);
		Assert.NotEqual(replacedTask, originalTask);


		// should change
		Assert.Equal(updateTask.ID, replacedTask.ID);
		Assert.Equal(updateTask.Name, replacedTask.Name);
		Assert.Equal(updateTask.Description, replacedTask.Description);
		Assert.False(replacedTask.IsCompleted);
		Assert.True(replacedTask.IsFavorite);

		// Remains unchanged
		Assert.Equal(originalTask.CreationDate, replacedTask.CreationDate);
		Assert.Equal(originalTask.LastDeletedTime, replacedTask.LastDeletedTime);
		Assert.Equal(originalTask.LastUpdatedTime, replacedTask.LastUpdatedTime);
		Assert.Equal(originalTask.IsDeleted, replacedTask.IsDeleted);

		// original object should not be modified
		Assert.Equal("task name", originalTask.Name);
	}

}
