using TodoAPI.API.Models;

namespace TodoAPI.API.Repositories;

public interface ITodoTaskRepository : IGenericRepository<TodoTask, int>
{

    // Get
    public IQueryable<TodoTask> GetPendings(int limit = 50);
    public IQueryable<TodoTask> GetCompleteds(int limit = 50);

    // Update
    public Task<TodoTask?> SetCompleted(int id, bool completed);


    // RawSQL Test
    public Task<TodoTask?> RawSQL_GetById(int id);
    public Task<TodoTask?> RawSQLWithDBSet_GetById(int id);
}
