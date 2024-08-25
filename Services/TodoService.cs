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

    public Task<List<TodoTask>> GetCompletedTasks()
    {
        return dbHandler.GetCompletedTasks();
    }

    public Task<List<TodoTask>> GetPendingTasks()
    {
        return dbHandler.GetPendingTasks();
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
