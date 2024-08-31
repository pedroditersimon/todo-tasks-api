using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoTaskController(ITodoTaskRepository taskRepository) : ControllerBase
{

    readonly ITodoTaskRepository taskRepository = taskRepository;

    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetAll()
        => await taskRepository.GetAllTask();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetByID(int id)
    {
        TodoTask? task = await taskRepository.GetTask(id);
        if (task == null)
            return NotFound();

        return task;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoTask>>> GetPendings()
        => await taskRepository.GetPendingTasks();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoTask>>> GetCompleteds()
        => await taskRepository.GetCompletedTasks();

    [HttpPost]
    public async Task<ActionResult<TodoTask?>> Create(TodoTask task)
        => await taskRepository.CreateTask(task);

    [HttpPut]
    public async Task<ActionResult<TodoTask>> Update(TodoTask task)
    {
        TodoTask? updatedTask = await taskRepository.UpdateTask(task);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpPut(nameof(SetCompleted))]
    public async Task<ActionResult<TodoTask>> SetCompleted(int id, bool completed)
    {
        TodoTask? updatedTask = await taskRepository.SetCompletedTask(id, completed);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpPut(nameof(SetTaskGoal))]
    public async Task<ActionResult<TodoTask>> SetTaskGoal(int taskID, int goalID)
    {
        TodoTask? updatedTask = await taskRepository.SetTaskGoal(taskID, goalID);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoTask? task = await taskRepository.GetTask(id);
        if (task == null)
            return NotFound();

        bool deleted = await taskRepository.DeleteTask(id);

        if (!deleted)
            return Conflict();

        return Ok();
    }


}
