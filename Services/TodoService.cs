using TodoAPI.Models;

namespace TodoAPI.Services;

public class TodoService(ITodoDBHandler dbHandler) : ITodoDBHandler
{
    readonly ITodoDBHandler dbHandler = dbHandler;

    public Task<TodoTask?> CreateTask(TodoTask task)
    {
        return dbHandler.CreateTask(task);
    }

    public Task<bool> DeleteTask(int id)
    {
        return dbHandler.DeleteTask(id);
    }

    public Task<List<TodoTask>> GetAllTask(int limit = 50)
    {
        return dbHandler.GetAllTask(limit);
    }

    public Task<List<TodoTask>> GetCompletedTasks(int limit = 50)
    {
        return dbHandler.GetCompletedTasks(limit);
    }

    public Task<List<TodoTask>> GetPendingTasks(int limit = 50)
    {
        return dbHandler.GetPendingTasks(limit);
    }

    public Task<TodoTask?> GetTask(int id)
    {
        return dbHandler.GetTask(id);
    }

    public Task<TodoTask?> SetCompletedTask(int id, bool completed)
    {
        return dbHandler.SetCompletedTask(id, completed);
    }

    public Task<TodoTask?> UpdateTask(TodoTask task)
    {
        return dbHandler.UpdateTask(task);
    }
}
