using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

public class GoalServiceTests
{
	[Fact]
	public async Task GoalIsCompletedStatusUpdated_WhenTaskIsCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated task
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.GoalService.AddTask(goal.ID, task.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task.IsCompleted = true;
		var updatedTask = await unitOfWork.TaskService.Update(task);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(updatedGoal);
		Assert.True(updatedGoal.IsCompleted, "The goal should be marked as completed when all its tasks are completed.");
	}

	[Fact]
	public async Task GoalIsCompletedStatusUpdated_WhenTaskIsNotCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated task
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.GoalService.AddTask(goal.ID, task.ID);
		await unitOfWork.Save();

		// Act: mark the task as not completed
		task.IsCompleted = false;
		var updatedTask = await unitOfWork.TaskService.Update(task);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now not completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when all its tasks are not completed.");
	}

	[Fact]
	public async Task GoalIsCompletedStatusNotUpdated_WhenOtherTaskIsCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated task
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		// Dont associate
		await unitOfWork.Save();

		// Act: mark the task as completed
		task.IsCompleted = true;
		var updatedTask = await unitOfWork.TaskService.Update(task);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is still not completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when other tasks are completed.");
	}
}
