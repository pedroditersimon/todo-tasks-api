using Microsoft.EntityFrameworkCore;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

// Class containing tests for ordering tasks and goals
public class OrderTests
{
	[Fact]
	public async Task OrderTasksByCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create tasks
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };
		var task3 = new TodoTask { ID = 3 };

		// Add tasks to the context
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.TaskService.Create(task3);
		await unitOfWork.Save();

		// Act: mark the task as completed (task with ID 2)
		task2.IsCompleted = true;
		await unitOfWork.TaskService.Update(task2);

		// Save changes
		await unitOfWork.Save();

		// Get all tasks to verify their order
		List<TodoTask>? tasks = await unitOfWork.TaskService.GetAll().ToListAsync();

		// Assert: verify the order based on completion status
		Assert.NotNull(tasks);
		Assert.Equal(3, tasks.Count);
		Assert.Equal(task2.ID, tasks[0].ID); // The completed task should be first
	}

	[Fact]
	public async Task OrderGoalsByCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create goals
		var goal1 = new TodoGoal { ID = 1 };
		var goal2 = new TodoGoal { ID = 2 };
		var goal3 = new TodoGoal { ID = 3 };

		// Add goals to the context
		await unitOfWork.GoalService.Create(goal1);
		await unitOfWork.GoalService.Create(goal2);
		await unitOfWork.GoalService.Create(goal3);
		await unitOfWork.Save();

		// Act: mark the goal as completed (goal with ID 2)
		goal2.IsCompleted = true;
		await unitOfWork.GoalService.Update(goal2);

		// Save changes
		await unitOfWork.Save();

		// Get all goals to verify their order
		List<TodoGoal>? goals = await unitOfWork.GoalService.GetAll();

		// Assert: verify the order based on completion status
		Assert.NotNull(goals);
		Assert.Equal(3, goals.Count);
		Assert.Equal(goal2.ID, goals[0].ID); // The completed goal should be first
	}

	[Fact]
	public async Task OrderTasksByFavorite()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create tasks
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };
		var task3 = new TodoTask { ID = 3 };

		// Add tasks to the context
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.TaskService.Create(task3);
		await unitOfWork.Save();

		// Act: mark the task as favorite (task with ID 2)
		task2.IsFavorite = true;
		await unitOfWork.TaskService.Update(task2);

		// Save changes
		await unitOfWork.Save();

		// Get all tasks to verify their order
		List<TodoTask>? tasks = await unitOfWork.TaskService.GetAll().ToListAsync();

		// Assert: verify the order based on favorite status
		Assert.NotNull(tasks);
		Assert.Equal(3, tasks.Count);
		Assert.Equal(task2.ID, tasks[0].ID); // The favorite task should be first
	}

	[Fact]
	public async Task OrderGoalsByFavorite()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create goals
		var goal1 = new TodoGoal { ID = 1 };
		var goal2 = new TodoGoal { ID = 2 };
		var goal3 = new TodoGoal { ID = 3 };

		// Add goals to the context
		await unitOfWork.GoalService.Create(goal1);
		await unitOfWork.GoalService.Create(goal2);
		await unitOfWork.GoalService.Create(goal3);
		await unitOfWork.Save();

		// Act: mark the goal as favorite (goal with ID 2)
		goal2.IsFavorite = true;
		await unitOfWork.GoalService.Update(goal2);

		// Save changes
		await unitOfWork.Save();

		// Get all goals to verify their order
		List<TodoGoal>? goals = await unitOfWork.GoalService.GetAll();

		// Assert: verify the order based on favorite status
		Assert.NotNull(goals);
		Assert.Equal(3, goals.Count);
		Assert.Equal(goal2.ID, goals[0].ID); // The favorite goal should be first
	}

	[Fact]
	public async Task OrderTasksByFavoriteAndCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Act: Create tasks and mark as favorite and completed
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2, IsCompleted = true };
		var task3 = new TodoTask { ID = 3, IsFavorite = true, IsCompleted = true };
		var task4 = new TodoTask { ID = 4, IsFavorite = true };

		// Add tasks to the context
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.TaskService.Create(task3);
		await unitOfWork.TaskService.Create(task4);

		// Save changes
		await unitOfWork.Save();

		// Get all tasks to verify their order
		List<TodoTask>? tasks = await unitOfWork.TaskService.GetAll().ToListAsync();

		// Assert: verify the order based on favorite and completed status
		Assert.NotNull(tasks);
		Assert.Equal(4, tasks.Count);
		Assert.Equal(task3.ID, tasks[0].ID); // Verify that task3 (favorite and completed) is first
		Assert.Equal(task4.ID, tasks[1].ID); // Verify that task4 (favorite) is second
		Assert.Equal(task2.ID, tasks[2].ID); // Verify that task2 (completed) is third
		Assert.Equal(task1.ID, tasks[3].ID); // Verify that task1 (not completed, not favorite) is last
	}

	[Fact]
	public async Task OrderGoalsByFavoriteAndCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create goals
		// Act: Create goals and mark as favorite and completed
		var goal1 = new TodoGoal { ID = 1 };
		var goal2 = new TodoGoal { ID = 2, IsCompleted = true };
		var goal3 = new TodoGoal { ID = 3, IsFavorite = true, IsCompleted = true };
		var goal4 = new TodoGoal { ID = 4, IsFavorite = true };

		// Add goals to the context
		await unitOfWork.GoalService.Create(goal1);
		await unitOfWork.GoalService.Create(goal2);
		await unitOfWork.GoalService.Create(goal3);
		await unitOfWork.GoalService.Create(goal4);

		// Save changes
		await unitOfWork.Save();

		// Get all goals to verify their order
		List<TodoGoal>? goals = await unitOfWork.GoalService.GetAll();

		// Assert: verify the order based on favorite and completed status
		Assert.NotNull(goals);
		Assert.Equal(4, goals.Count);
		Assert.Equal(goal3.ID, goals[0].ID); // Verify that goal3 (favorite and completed) is first
		Assert.Equal(goal4.ID, goals[1].ID); // Verify that goal4 (favorite) is second
		Assert.Equal(goal2.ID, goals[2].ID); // Verify that goal2 (completed) is third
		Assert.Equal(goal1.ID, goals[3].ID); // Verify that goal1 (not completed, not favorite) is last
	}

}
