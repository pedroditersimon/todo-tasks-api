using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

public class GoalServiceTests
{
	[Fact]
	public async Task StatusIsUpdated_WhenTaskIsCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.GoalService.AddTask(goal.ID, task1.ID);
		await unitOfWork.GoalService.AddTask(goal.ID, task2.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task1.IsCompleted = true;
		task2.IsCompleted = true;
		await unitOfWork.TaskService.Update(task1);
		await unitOfWork.TaskService.Update(task2);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalService.GetByID(goal.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(updatedGoal);
		Assert.True(updatedGoal.IsCompleted, "The goal should be marked as completed when all its tasks are completed.");
		Assert.Equal(100, updatedGoal.CompletedPercent);
	}

	[Fact]
	public async Task StatusUpdated_WhenTaskIsNotCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.GoalService.AddTask(goal.ID, task1.ID);
		await unitOfWork.GoalService.AddTask(goal.ID, task2.ID);
		await unitOfWork.Save();

		// Act: mark the task as not completed
		task1.IsCompleted = false;
		var updatedTask = await unitOfWork.TaskService.Update(task1);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now not completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when all its tasks are not completed.");
		Assert.Equal(0, updatedGoal.CompletedPercent);
	}

	[Fact]
	public async Task StatusNotUpdated_WhenOtherTaskIsCompleted()
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
		Assert.Equal(0, updatedGoal.CompletedPercent);
	}

	[Fact]
	public async Task StatusIsUpdated_WhenCompletedTaskIsRemoved()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task1 = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.GoalService.AddTask(goal.ID, task1.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task1.IsCompleted = true;
		await unitOfWork.TaskService.Update(task1);
		// Save changes
		await unitOfWork.Save();

		// Act: dissociate the first task
		await unitOfWork.GoalService.RemoveTask(goal.ID, task1.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when a completed task is dissociated.");
		Assert.Equal(0, updatedGoal.CompletedPercent);
	}


	[Fact]
	public async Task StatusIsUpdated_WhenCompletedTaskIsSoftDeleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.GoalService.AddTask(goal.ID, task.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task.IsCompleted = true;
		await unitOfWork.TaskService.Update(task);
		// Save changes
		await unitOfWork.Save();

		// Act: delete the first task
		await unitOfWork.TaskService.SoftDelete(task.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when a completed task is soft deleted.");
		Assert.Equal(0, updatedGoal.CompletedPercent);
	}

	[Fact]
	public async Task StatusIsUpdated_WhenCompletedTaskIsHardDeleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.GoalService.AddTask(goal.ID, task.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task.IsCompleted = true;
		await unitOfWork.TaskService.Update(task);
		// Save changes
		await unitOfWork.Save();

		// Act: delete the first task
		await unitOfWork.TaskService.HardDelete(task.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		TodoGoal? updatedGoal = await unitOfWork.GoalRepository.GetByID(goal.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(updatedGoal);
		Assert.False(updatedGoal.IsCompleted, "The goal should not be marked as completed when a completed task is hard deleted.");
		Assert.Equal(0, updatedGoal.CompletedPercent);
	}



	[Fact]
	public async Task GetPendings()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.GoalService.AddTask(goal.ID, task1.ID);
		await unitOfWork.GoalService.AddTask(goal.ID, task2.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task1.IsCompleted = true;
		await unitOfWork.TaskService.Update(task1);
		// task2 still with IsCompleted in false

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		List<TodoGoal> pendingGoals = await unitOfWork.GoalService.GetPendings();

		// Assert: verify that the goal is now completed
		Assert.NotNull(pendingGoals);
		Assert.Single(pendingGoals);
		Assert.Contains(pendingGoals, g => g.ID.Equals(goal.ID));
		Assert.All(pendingGoals, g => Assert.False(g.IsCompleted));
	}


	[Fact]
	public async Task GetCompleteds()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task1 = new TodoTask { ID = 1 };
		var task2 = new TodoTask { ID = 2 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task1);
		await unitOfWork.TaskService.Create(task2);
		await unitOfWork.GoalService.AddTask(goal.ID, task1.ID);
		await unitOfWork.GoalService.AddTask(goal.ID, task2.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task1.IsCompleted = true;
		task2.IsCompleted = true;
		await unitOfWork.TaskService.Update(task1);
		await unitOfWork.TaskService.Update(task2);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		List<TodoGoal> completedGoals = await unitOfWork.GoalService.GetCompleteds();

		// Assert: verify that the goal is now completed
		Assert.NotNull(completedGoals);
		Assert.Single(completedGoals);
		Assert.Contains(completedGoals, g => g.ID.Equals(goal.ID));
		Assert.All(completedGoals, g => Assert.True(g.IsCompleted));
	}



	[Fact]
	public async Task GetAllByTask_WhenTaskIsCompleted()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal1 = new TodoGoal { ID = 1 };
		var goal2 = new TodoGoal { ID = 2 };
		var task = new TodoTask { ID = 1 };

		// Add the goal and the task to the context
		await unitOfWork.GoalService.Create(goal1);
		await unitOfWork.GoalService.Create(goal2);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.GoalService.AddTask(goal1.ID, task.ID);
		await unitOfWork.GoalService.AddTask(goal2.ID, task.ID);
		await unitOfWork.Save();

		// Act: mark the task as completed
		task.IsCompleted = true;
		await unitOfWork.TaskService.Update(task);

		// Save changes
		await unitOfWork.Save();

		// Get the goal to verify its status
		List<TodoGoal> completedGoals = await unitOfWork.GoalService.GetAllByTask(task.ID);

		// Assert: verify that the goal is now completed
		Assert.NotNull(completedGoals);
		Assert.Equal(2, completedGoals.Count);
		Assert.Contains(completedGoals, g => g.ID.Equals(goal1.ID));
		Assert.Contains(completedGoals, g => g.ID.Equals(goal2.ID));
		Assert.All(completedGoals, g => Assert.True(g.IsCompleted));
	}


}
