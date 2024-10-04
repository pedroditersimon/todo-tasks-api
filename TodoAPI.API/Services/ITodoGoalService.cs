using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface ITodoGoalService : IGenericService<TodoGoal, int>
{
	// Get

	public IQueryable<TodoGoal> GetPendings(int limit = 0);
	public IQueryable<TodoGoal> GetCompleteds(int limit = 0);

	public IQueryable<TodoGoal> GetAllByTask(int taskID, int limit = 0);

	// Update
	public Task<bool> AddTask(int goalID, int taskID);
	public Task<bool> RemoveTask(int goalID, int taskID);

	public Task<bool> UpdateAllCompletedStatusByTask(int taskID);
	public Task<bool> UpdateCompletedStatus(int goalID);
}
