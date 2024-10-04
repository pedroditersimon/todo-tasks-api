﻿using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoGoalService : GenericService<TodoGoal, int>, ITodoGoalService
{
	readonly ITodoGoalRepository _repository;

	public TodoGoalService(ITodoGoalRepository repository)
		: base(repository)
	{
		_repository = repository;
	}

	#region Get
	public async Task<TodoGoal?> GetByIDWithTasks(int id)
		=> await _repository.GetAll()
			.Include(g => g.Tasks)
			.SingleOrDefaultAsync(g => g.ID.Equals(id));

	public IQueryable<TodoGoal> GetAllWithTasks(int limit = 50)
		=> _repository.GetAll().Include(g => g.Tasks);

	public IQueryable<TodoGoal> GetPendings(int limit = 50)
		=> _repository.GetAll()
			.Where((g) => g.Tasks.Any(task => !task.IsCompleted))
			.OrderBy(g => g.ID)
			.Take(limit);


	public IQueryable<TodoGoal> GetCompleteds(int limit = 50)
		=> _repository.GetAll()
			.Where((g) => !g.Tasks.Any(task => !task.IsCompleted))
			.OrderBy(g => g.ID)
			.Take(limit);
	#endregion

	#region Update
	public async Task<bool> AddTask(int goalID, TodoTask task)
	{
		TodoGoal? goal = await GetByIDWithTasks(goalID);
		if (goal == null)
			return false;

		goal.Tasks.Add(task);
		return true;
	}

	public async Task<bool> RemoveTask(int goalID, int taskID)
	{
		TodoGoal? goal = await GetByIDWithTasks(goalID);
		if (goal == null)
			return false;

		TodoTask? task = goal.Tasks.SingleOrDefault(t => t.ID.Equals(taskID));
		if (task == null)
			return false;

		return goal.Tasks.Remove(task);
	}

	public async Task<bool> UpdateCompletedStatus(int goalID)
	{
		TodoGoal? goal = await GetByIDWithTasks(goalID);
		if (goal == null)
			return false;

		goal.IsCompleted = goal.Tasks.All((t) => t.IsCompleted);

		TodoGoal? updatedGoal = await Update(goal);
		if (updatedGoal == null)
			return false;

		return true;
	}
	#endregion

}
