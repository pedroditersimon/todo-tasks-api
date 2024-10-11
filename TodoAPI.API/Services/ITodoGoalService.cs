using TodoAPI.API.Interfaces;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface ITodoGoalService : IGenericService<TodoGoal, int>, ISaveable
{
	// Get
	public new Task<List<TodoGoal>> GetAll(int limit = 0);
	public Task<List<TodoGoal>> GetPendings(int limit = 0);
	public Task<List<TodoGoal>> GetCompleteds(int limit = 0);

	public Task<List<TodoGoal>> GetAllByTask(int taskID, int limit = 0);

	// Update
	public Task<bool> AddTask(int goalID, int taskID);
	public Task<bool> RemoveTask(int goalID, int taskID);

}
