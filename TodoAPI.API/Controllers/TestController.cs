using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("Test")]
public class TestController(DbContext dbContext, IUnitOfWork unitOfWork) : ControllerBase
{
	#region EntityLoadingTest
	[HttpGet("EntityLoadingTest/" + nameof(EagerLoading))]
	public async Task<ActionResult<TodoGoal?>> EagerLoading(int goalID = 1)
	{
		return unitOfWork.GoalRepository.GetAll()
			.Include(g => g.Tasks)
			.SingleOrDefault(g => g.ID == goalID);
	}

	[HttpGet("EntityLoadingTest/" + nameof(LazyLoading))]
	public async Task<ActionResult<TodoGoal>> LazyLoading(int goalID = 1)
	{
		TodoGoal? goal = unitOfWork.GoalRepository.GetAll().SingleOrDefault(g => g.ID == goalID);
		if (goal == null)
			return NotFound();

		// must have: UseLazyLoadingProxies() from Microsoft.EntityFrameworkCore.Proxies
		ICollection<TodoTask> goalTasks = goal.Tasks.ToList();

		return goal;
	}


	[HttpGet("EntityLoadingTest/" + nameof(ExplicitLoading))]
	public async Task<ActionResult<TodoGoal>> ExplicitLoading(int goalID = 1)
	{
		TodoGoal? goal = unitOfWork.GoalRepository.GetAll().SingleOrDefault(g => g.ID == goalID);
		if (goal == null)
			return NotFound();

		// this doesnt require UseLazyLoadingProxies and can bypass it
		await dbContext.Entry(goal)
			.Collection(g => g.Tasks)
			.LoadAsync();

		return goal;
	}
	#endregion


	#region RawSQL
	[HttpGet("RawSQL/GetById")]
	public async Task<ActionResult<TodoTask?>> RawSQL_GetById(int id)
	{
		TodoTask? task = await unitOfWork.TaskService.RawSQL_GetById(id);
		if (task == null)
			return NotFound();

		return task;
	}


	[HttpGet("RawSQLWithDBSet/GetById")]
	public async Task<ActionResult<TodoTask?>> RawSQLWithDBSet_GetById(int id)
	{
		TodoTask? task = await unitOfWork.TaskService.RawSQLWithDBSet_GetById(id);
		if (task == null)
			return NotFound();

		return task;
	}
	#endregion

	#region RawSQL
	[HttpGet("StoredProcedures/GetById")]
	public async Task<ActionResult<TodoTask?>> StoredProcedures_GetById(int id)
	{
		TodoTask? task = await unitOfWork.TaskService.StoredProcedure_GetByID(id);
		if (task == null)
			return NotFound();

		return task;
	}

	#endregion

	[HttpGet("FirstLevelCache/GetById")]
	public async Task<ActionResult<TodoTask?>> FirstLevelCache_GetById(int id)
	{
		// You may notice that the second query takes less than 1 ms. (enable ef Logging in appsettings)
		TodoTask? task = await unitOfWork.TaskRepository.GetByID(id);
		TodoTask? task2 = await unitOfWork.TaskRepository.GetByID(id);
		if (task == null || task2 == null)
			return NotFound();

		return task;
	}


	[HttpGet("Delayed/UpdateTaskName")]
	public async Task<ActionResult<TodoTask?>> Delayed_UpdateTaskName(int id, string newName)
	{
		TodoTask? task = await unitOfWork.TaskRepository.GetByID(id);
		if (task == null)
			return NotFound();

		// 10 seconds delay
		await Task.Delay(10000);

		// update and save
		task.Name = newName;

		TodoTask? updatedTask = await unitOfWork.TaskRepository.Update(task);
		bool saved = await unitOfWork.Save() > 0;

		// error
		if (updatedTask == null || !saved)
			return Conflict();

		return updatedTask;
	}


	[HttpGet("Transaction/InsertTask/{forceFail}")]
	public async Task<ActionResult<TodoTask?>> Transaction_InsertTask(bool forceFail)
	{
		using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();

		// only works with dbContext set in the same scope, not in repositories
		EntityEntry<TodoTask> insertedTask = dbContext.Set<TodoTask>().Add(new TodoTask
		{
			Name = "Transactional Test"
		});

		// only works in the same scope, not with unitOfWork.Save()
		bool saved = await dbContext.SaveChangesAsync() > 0;

		// error
		if (forceFail || insertedTask == null || !saved)
		{
			await transaction.RollbackAsync();
			// transaction.Dispose(); // this will dispose automatically with the scope

			return Conflict();
		}

		transaction.Commit();
		return insertedTask.Entity;
	}

}

