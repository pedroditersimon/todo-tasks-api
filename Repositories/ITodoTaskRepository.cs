using TodoAPI.Models;

namespace TodoAPI.Repositories;

public interface ITodoTaskRepository
{

    // Get
    public Task<TodoTask?> GetTask(int id);
    public Task<List<TodoTask>> GetAllTask(int limit = 50);
    public Task<List<TodoTask>> GetPendingTasks(int limit = 50);
    public Task<List<TodoTask>> GetCompletedTasks(int limit = 50);

    // Create
    public Task<TodoTask?> CreateTask(TodoTask task);

    // Delete
    public Task<bool> DeleteTask(int id);

    // Update
    public Task<TodoTask?> UpdateTask(TodoTask task);
    public Task<TodoTask?> SetCompletedTask(int id, bool completed);
    public Task<TodoTask?> SetTaskGoal(int taskID, int goalID);
}
