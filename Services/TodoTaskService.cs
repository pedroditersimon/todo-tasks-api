using TodoAPI.Models;

namespace TodoAPI.Services;

public class TodoTaskService(ITodoTaskDBHandler taskDbHandler) : ITodoTaskDBHandler
{
    readonly ITodoTaskDBHandler taskDbHandler = taskDbHandler;


    public Task<TodoTask?> CreateTask(TodoTask task)
    {
        return taskDbHandler.CreateTask(task);
    }

    public Task<bool> DeleteTask(int id)
    {
        return taskDbHandler.DeleteTask(id);
    }

    public Task<List<TodoTask>> GetAllTask(int limit = 50)
    {
        return taskDbHandler.GetAllTask(limit);
    }

    public Task<List<TodoTask>> GetCompletedTasks(int limit = 50)
    {
        return taskDbHandler.GetCompletedTasks(limit);
    }

    public Task<List<TodoTask>> GetPendingTasks(int limit = 50)
    {
        return taskDbHandler.GetPendingTasks(limit);
    }

    public Task<TodoTask?> GetTask(int id)
    {
        return taskDbHandler.GetTask(id);
    }

    public Task<TodoTask?> SetCompletedTask(int id, bool completed)
    {
        return taskDbHandler.SetCompletedTask(id, completed);
    }

    public Task<TodoTask?> SetTaskGoal(int taskID, int goalID)
    {
        return taskDbHandler.SetTaskGoal(taskID, goalID);
    }

    public Task<TodoTask?> UpdateTask(TodoTask task)
    {
        return taskDbHandler.UpdateTask(task);
    }
}
