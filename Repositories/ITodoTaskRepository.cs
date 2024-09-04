using TodoAPI.Models;

namespace TodoAPI.Repositories;

public interface ITodoTaskRepository : IGenericRepository<TodoTask, int>
{

    // Get
    public IQueryable<TodoTask> GetPendings(int limit = 50);
    public IQueryable<TodoTask> GetCompleteds(int limit = 50);

    // Update
    public Task<TodoTask?> SetCompleted(int id, bool completed);
}
