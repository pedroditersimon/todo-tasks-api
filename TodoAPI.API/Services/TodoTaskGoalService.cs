using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoTaskGoalService : ITodoTaskGoalService
{

	readonly ITodoTaskGoalRepository _repository;

	public TodoTaskGoalService(ITodoTaskGoalRepository repository)
	{
		_repository = repository;
	}



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


	public async Task<bool> Associate(int taskID, int goalID)
	{
		bool exists = await _repository.Exists(taskID, goalID);
		if (exists)
			return false;

		TodoTaskGoal? relation = _repository.Create(taskID, goalID);
		return relation != null;
	}

	public async Task<bool> Dissociate(int taskID, int goalID)
	{
		bool exists = await _repository.Exists(taskID, goalID);
		if (!exists)
			return false;

		return await _repository.Delete(taskID, goalID);
	}


}
