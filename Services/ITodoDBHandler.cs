using TodoAPI.Models;

namespace TodoAPI.Services;

public interface ITodoDBHandler
{
    // Get
    public Task<TodoTask?> GetTask(int id);
    public Task<List<TodoTask>> GetPendingTasks();
    public Task<List<TodoTask>> GetCompletedTasks();

    // Create
    public Task<TodoTask?> CreateTask(TodoTask task);

    // Delete
    public Task<bool> DeleteTask(int id);

    // Update
    public Task<TodoTask?> UpdateTask(TodoTask task);
    public Task<TodoTask?> SetCompletedTask(int id, bool completed);
}
