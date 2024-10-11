using TodoAPI.Data.Events.TaskGoal;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface ITodoTaskGoalService
{

	public Task<TodoTaskGoal?> GetByID(int goalID, int taskID);
	public IQueryable<TodoTaskGoal> GetByGoalID(int goalID, int limit = 0);

	public IQueryable<TodoTaskGoal> GetByTaskID(int taskID, int limit = 0);


	public IQueryable<TodoTask> GetTasksByGoalID(int goalID, int limit = 0);

	public IQueryable<TodoGoal> GetGoalsByTaskID(int taskID, int limit = 0);

	public Task<bool> Associate(int goalID, int taskID);
	public Task<bool> Dissociate(int goalID, int taskID);

	public Task<bool> DissociateAllByTaskID(int taskID);
	public Task<bool> DissociateAllByGoalID(int goalID);


	// Events
	public event EventHandler<AssociateEventArgs> OnAssociate;
	public event EventHandler<DissociateEventArgs> OnDissociate;
}
