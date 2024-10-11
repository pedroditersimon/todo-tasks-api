using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Events.TaskGoal;
using TodoAPI.Data.Models;
using TodoAPI.Data.Events;

namespace TodoAPI.API.Services;

public class TodoTaskGoalService : ITodoTaskGoalService
{

	readonly ITodoTaskGoalRepository _repository;

	public event AsyncEventHandler<AssociateEventArgs> OnAssociate;
	public event AsyncEventHandler<DissociateEventArgs> OnDissociate;

	public TodoTaskGoalService(ITodoTaskGoalRepository repository)
	{
		_repository = repository;
	}

	public Task<TodoTaskGoal?> GetByID(int goalID, int taskID)
		=> _repository.GetByID(goalID, taskID);

	public IQueryable<TodoTaskGoal> GetByGoalID(int goalID, int limit = 0)
		=> _repository.GetAll()
			.Where(tg => tg.TodoGoalID == goalID)
			.TakeLimit(limit);

	public IQueryable<TodoTaskGoal> GetByTaskID(int taskID, int limit = 0)
		=> _repository.GetAll()
			.Where(tg => tg.TodoTaskID == taskID)
			.TakeLimit(limit);


	public IQueryable<TodoGoal> GetGoalsByTaskID(int taskID, int limit = 0)
		=> _repository.GetAll()
			.Where(tg => tg.TodoTaskID == taskID)
			.TakeLimit(limit)
			.Include(tg => tg.TodoGoal)
			.Select(tg => tg.TodoGoal);


	public IQueryable<TodoTask> GetTasksByGoalID(int goalID, int limit = 0)
		=> _repository.GetAll()
			.Where(tg => tg.TodoGoalID == goalID)
			.TakeLimit(limit)
			.Include(tg => tg.TodoTask)
			.Select(tg => tg.TodoTask);


	public async Task<bool> Associate(int goalID, int taskID)
	{
		bool exists = await _repository.Exists(goalID, taskID);
		if (exists)
			return false;

		TodoTaskGoal? relation = _repository.Create(goalID, taskID);
		if (relation == null)
			return false;

		// trigger event
		await OnAssociate(this, new AssociateEventArgs(goalID, taskID));
		return true;
	}

	public async Task<bool> Dissociate(int goalID, int taskID)
	{
		bool exists = await _repository.Exists(goalID, taskID);
		if (!exists)
			return false;

		bool success = await _repository.Delete(goalID, taskID);
		if (!success)
			return false;

		// trigger event
		await OnDissociate(this, new DissociateEventArgs(goalID, taskID));
		return true;
	}


	public async Task<bool> DissociateAllByTaskID(int taskID)
	{
		// get goals associated with this task
		List<TodoGoal> goals = await GetGoalsByTaskID(taskID).ToListAsync();

		// Remove associations
		foreach (var g in goals)
		{
			bool success = await Dissociate(g.ID, taskID);
			if (!success)
				return false;
		}

		return true;
	}

	public async Task<bool> DissociateAllByGoalID(int goalID)
	{
		// get tasks associated with this goal
		List<TodoTask> tasks = await GetTasksByGoalID(goalID).ToListAsync();

		// Remove associations
		foreach (var t in tasks)
		{
			bool success = await Dissociate(goalID, t.ID);
			if (!success)
				return false;
		}

		return true;
	}

}
