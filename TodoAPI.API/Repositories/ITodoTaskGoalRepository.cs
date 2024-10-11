using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public interface ITodoTaskGoalRepository
{
	public Task<bool> Exists(int goalID, int taskID);


	public Task<TodoTaskGoal?> GetByID(int goalID, int taskID);
	public IQueryable<TodoTaskGoal> GetAll(int limit = 0);

	public TodoTaskGoal? Create(int goalID, int taskID);

	public Task<bool> Delete(int goalID, int taskID);
}
