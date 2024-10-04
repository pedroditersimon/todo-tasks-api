using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public interface ITodoGoalRepository : IGenericRepository<TodoGoal, int>
{

    // Get
    public Task<TodoGoal?> GetByIDWithTasks(int id);
    public IQueryable<TodoGoal> GetAllWithTasks(int limit = 50);

    public IQueryable<TodoGoal> GetPendings(int limit = 50);
    public IQueryable<TodoGoal> GetCompleteds(int limit = 50);

    // Update
    public Task<bool> AddTask(int goalID, TodoTask task);
    public Task<bool> RemoveTask(int goalID, int taskID);
}
