using TodoAPI.Models;

namespace TodoAPI.Repositories;

public class TodoTaskRepository(ITodoTaskRepository taskRepository) : ITodoTaskRepository
{
    readonly ITodoTaskRepository taskRepository = taskRepository;


    public Task<TodoTask?> CreateTask(TodoTask task)
    {
        return taskRepository.CreateTask(task);
    }

    public Task<bool> DeleteTask(int id)
    {
        return taskRepository.DeleteTask(id);
    }

    public Task<List<TodoTask>> GetAllTask(int limit = 50)
    {
        return taskRepository.GetAllTask(limit);
    }

    public Task<List<TodoTask>> GetCompletedTasks(int limit = 50)
    {
        return taskRepository.GetCompletedTasks(limit);
    }

    public Task<List<TodoTask>> GetPendingTasks(int limit = 50)
    {
        return taskRepository.GetPendingTasks(limit);
    }

    public Task<TodoTask?> GetTask(int id)
    {
        return taskRepository.GetTask(id);
    }

    public Task<TodoTask?> SetCompletedTask(int id, bool completed)
    {
        return taskRepository.SetCompletedTask(id, completed);
    }

    public Task<TodoTask?> SetTaskGoal(int taskID, int goalID)
    {
        return taskRepository.SetTaskGoal(taskID, goalID);
    }

    public Task<TodoTask?> UpdateTask(TodoTask task)
    {
        return taskRepository.UpdateTask(task);
    }
}
