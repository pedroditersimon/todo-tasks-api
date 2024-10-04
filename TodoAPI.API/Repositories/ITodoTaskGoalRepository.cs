using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public interface ITodoTaskGoalRepository
{
	public Task<bool> Exists(int taskID, int goalID);

	public Task<TodoTaskGoal?> GetByID(int taskID, int goalID);
	public IQueryable<TodoTaskGoal> GetAll(int limit = 0);

	public TodoTaskGoal? Create(int taskID, int goalID);

	public Task<bool> Delete(int taskID, int goalID);
}
