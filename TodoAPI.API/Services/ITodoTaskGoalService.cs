using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface ITodoTaskGoalService
{

	public Task<TodoTaskGoal?> GetByID(int taskID, int goalID);
	public IQueryable<TodoTaskGoal> GetByGoalID(int goalID, int limit = 0);

	public IQueryable<TodoTaskGoal> GetByTaskID(int taskID, int limit = 0);


	public IQueryable<TodoTask> GetTasksByGoalID(int goalID, int limit = 0);

	public IQueryable<TodoGoal> GetGoalsByTaskID(int taskID, int limit = 0);

	public Task<bool> Associate(int taskID, int goalID);
	public Task<bool> Dissociate(int taskID, int goalID);
}
