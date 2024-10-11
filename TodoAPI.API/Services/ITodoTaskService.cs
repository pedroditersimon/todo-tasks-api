using TodoAPI.Data.Events.Task;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface ITodoTaskService : IGenericService<TodoTask, int>
{
	// Get
	public IQueryable<TodoTask> GetAllByGoal(int goalID, int limit = 0);

	public IQueryable<TodoTask> GetPendings(int limit = 0);
	public IQueryable<TodoTask> GetCompleteds(int limit = 0);

	// Update
	public Task<TodoTask?> SetCompleted(int id, bool completed);


	// RawSQL Test
	public Task<TodoTask?> RawSQL_GetById(int id);
	public Task<TodoTask?> RawSQLWithDBSet_GetById(int id);

	// Stored Procedures Test
	public Task<TodoTask?> StoredProcedure_GetByID(int id);

	// Events
	public event EventHandler<TaskIsDeletedEventArgs> OnTaskIsDeleted;
	public event EventHandler<TaskIsUpdatedEventArgs> OnTaskIsUpdated;
}
