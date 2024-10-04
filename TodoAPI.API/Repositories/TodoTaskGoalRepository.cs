using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoTaskGoalRepository : ITodoTaskGoalRepository
{
	readonly TodoDBContext _dBContext;
	protected DbSet<TodoTaskGoal> Entities => _dBContext.Set<TodoTaskGoal>();

	public TodoTaskGoalRepository(TodoDBContext dBContext)
	{
		_dBContext = dBContext;
	}

	public async Task<bool> Exists(int taskID, int goalID)
		=> await GetByID(taskID, goalID) != null;

	public async Task<TodoTaskGoal?> GetByID(int taskID, int goalID)
	{
		return await Entities.SingleOrDefaultAsync(
			tg => tg.TodoTaskID.Equals(taskID) && tg.TodoGoalID.Equals(goalID));
	}

	public IQueryable<TodoTaskGoal> GetAll(int limit = 0)
		=> Entities.TakeLimit(limit);

	public TodoTaskGoal? Create(int taskID, int goalID)
	{
		TodoTaskGoal relation = new()
		{
			TodoTaskID = taskID,
			TodoGoalID = goalID
		};

		return Entities.Add(relation).Entity;
	}

	public async Task<bool> Delete(int taskID, int goalID)
	{
		TodoTaskGoal? relation = await GetByID(taskID, goalID);
		if (relation == null)
			return false;

		Entities.Remove(relation);
		return true;
	}


}
