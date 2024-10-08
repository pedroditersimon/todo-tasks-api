using Microsoft.EntityFrameworkCore;
using TodoAPI.Data.Models;
using Xunit;

namespace TodoAPI.Tests;

public class TaskGoalServiceTests
{
	[Fact]
	public async Task RelationDoesExists_WhenAssociate()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Get the relation
		TodoTaskGoal? relation = await unitOfWork.TaskGoalService.GetByID(task.ID, goal.ID);

		// Assert: verify that the relation exists
		Assert.NotNull(relation);
		Assert.Equal(task.ID, relation.TodoTaskID);
		Assert.Equal(goal.ID, relation.TodoGoalID);
	}


	[Fact]
	public async Task RelationDoesNotExists_WhenDissociate()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Act: dissociate the task
		await unitOfWork.TaskGoalService.Dissociate(task.ID, goal.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the relation
		TodoTaskGoal? relation = await unitOfWork.TaskGoalService.GetByID(task.ID, goal.ID);

		// Assert: verify that the relation not exists
		Assert.Null(relation);
	}


	[Fact]
	public async Task RelationDoesExists_WhenAssociate_GetByTask()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Get the relation
		List<TodoTaskGoal> relations = await unitOfWork.TaskGoalService.GetByTaskID(task.ID).ToListAsync();

		// Assert: verify that the relation not exists
		Assert.NotNull(relations);
		Assert.NotEmpty(relations);
		Assert.Single(relations);
		Assert.Equal(task.ID, relations[0].TodoTaskID);
		Assert.Equal(goal.ID, relations[0].TodoGoalID);
	}

	[Fact]
	public async Task RelationDoesNotExists_WhenDissociate_GetByTask()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Act: dissociate the task
		await unitOfWork.TaskGoalService.Dissociate(task.ID, goal.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the relation
		List<TodoTaskGoal> relations = await unitOfWork.TaskGoalService.GetByTaskID(task.ID).ToListAsync();

		// Assert: verify that the relation not exists
		Assert.NotNull(relations);
		Assert.Empty(relations);
	}

	[Fact]
	public async Task RelationDoesExists_WhenAssociate_GetByGoal()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Get the relation
		List<TodoTaskGoal> relations = await unitOfWork.TaskGoalService.GetByGoalID(goal.ID).ToListAsync();

		// Assert: verify that the relation not exists
		Assert.NotNull(relations);
		Assert.NotEmpty(relations);
		Assert.Single(relations);
		Assert.Equal(task.ID, relations[0].TodoTaskID);
		Assert.Equal(goal.ID, relations[0].TodoGoalID);
	}



	[Fact]
	public async Task RelationDoesNotExists_WhenDissociate_GetByGoal()
	{
		// Arrange: set up the context and dependencies
		using var dbContext = TestsHelper.CreateDBContext();
		using var unitOfWork = TestsHelper.CreateUnitOfWork(dbContext);

		// Create a goal and an associated tasks
		var goal = new TodoGoal { ID = 1 };
		var task = new TodoTask { ID = 1 };

		// create relation
		await unitOfWork.GoalService.Create(goal);
		await unitOfWork.TaskService.Create(task);
		await unitOfWork.TaskGoalService.Associate(task.ID, goal.ID);
		await unitOfWork.Save();

		// Act: dissociate the task
		await unitOfWork.TaskGoalService.Dissociate(task.ID, goal.ID);
		// Save changes
		await unitOfWork.Save();

		// Get the relation
		List<TodoTaskGoal> relations = await unitOfWork.TaskGoalService.GetByGoalID(goal.ID).ToListAsync();

		// Assert: verify that the relation not exists
		Assert.NotNull(relations);
		Assert.Empty(relations);
	}

}
