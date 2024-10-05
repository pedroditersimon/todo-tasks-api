using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoGoalService : GenericService<TodoGoal, int>, ITodoGoalService
{
	readonly ITodoGoalRepository _repository;
	readonly ITodoTaskGoalService _taskGoalService;

	public TodoGoalService(ITodoGoalRepository repository, ITodoTaskGoalService taskGoalService)
		: base(repository)
	{
		_repository = repository;
		_taskGoalService = taskGoalService;
	}

	#region Get

	public IQueryable<TodoGoal> GetPendings(int limit = 0)
		=> _repository.GetAll()
			.Where((g) => !g.IsCompleted)
			.OrderBy(g => g.ID)
			.TakeLimit(limit);

	public IQueryable<TodoGoal> GetCompleteds(int limit = 0)
		=> _repository.GetAll()
			.Where((g) => g.IsCompleted)
			.OrderBy(g => g.ID)
			.TakeLimit(limit);

	public IQueryable<TodoGoal> GetAllByTask(int taskID, int limit = 0)
		=> _taskGoalService.GetGoalsByTaskID(taskID, limit);

	#endregion

	#region Update
	public async Task<bool> AddTask(int goalID, int taskID)
	{
		bool success = await _taskGoalService.Associate(goalID, taskID);
		if (!success)
			return false;

		bool successUpdatedStatus = await UpdateCompletedStatus(goalID);
		if (!successUpdatedStatus)
			return false;

		return true;
	}

	public async Task<bool> RemoveTask(int goalID, int taskID)
	{
		bool success = await _taskGoalService.Dissociate(goalID, taskID);
		if (!success)
			return false;

		bool successUpdatedStatus = await UpdateCompletedStatus(goalID);
		if (!successUpdatedStatus)
			return false;

		return true;
	}


	public async Task<bool> UpdateAllCompletedStatusByTask(int taskID)
	{
		List<TodoGoal> goals = await GetAllByTask(taskID).ToListAsync();
		foreach (var goal in goals)
		{
			bool success = await UpdateCompletedStatus(goal);
			if (!success)
				return false;
		}

		return true;
	}

	public async Task<bool> UpdateCompletedStatus(int goalID)
	{
		TodoGoal? goal = await GetByID(goalID);
		if (goal == null)
			return false;

		return await UpdateCompletedStatus(goal);
	}

	public async Task<bool> UpdateCompletedStatus(TodoGoal goal)
	{
		List<TodoTask> tasks = await _taskGoalService.GetTasksByGoalID(goal.ID).ToListAsync();
		if (tasks.Count == 0)
			return true;

		// calculate goal completed percent
		if (tasks.Count > 0)
		{
			int completedTasksCount = tasks.Count(t => t.IsCompleted);
			float percent = completedTasksCount / (float)tasks.Count;
			goal.CompletedPercent = MathF.Truncate(percent * 100);
		}
		else
		{
			goal.CompletedPercent = 0;
		}

		// calculate goal completed status
		goal.IsCompleted = goal.CompletedPercent >= 100.0f;


		TodoGoal? updatedGoal = await Update(goal);
		if (updatedGoal == null)
			return false;

		return true;
	}
	#endregion

}
