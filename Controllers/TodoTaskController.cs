using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoTaskController(TodoTaskService todoService) : ControllerBase
{

    readonly TodoTaskService todoService = todoService;

    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetAll()
        => await todoService.GetAllTask();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetByID(int id)
    {
        TodoTask? task = await todoService.GetTask(id);
        if (task == null)
            return NotFound();

        return task;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoTask>>> GetPendings()
        => await todoService.GetPendingTasks();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoTask>>> GetCompleteds()
        => await todoService.GetCompletedTasks();

    [HttpPost]
    public async Task<ActionResult<TodoTask?>> Create(TodoTask task)
        => await todoService.CreateTask(task);

    [HttpPut]
    public async Task<ActionResult<TodoTask>> Update(TodoTask task)
    {
        TodoTask? updatedTask = await todoService.UpdateTask(task);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpPut(nameof(SetCompleted))]
    public async Task<ActionResult<TodoTask>> SetCompleted(int id, bool completed)
    {
        TodoTask? updatedTask = await todoService.SetCompletedTask(id, completed);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpPut(nameof(SetTaskGoal))]
    public async Task<ActionResult<TodoTask>> SetTaskGoal(int taskID, int goalID)
    {
        TodoTask? updatedTask = await todoService.SetTaskGoal(taskID, goalID);
        if (updatedTask == null)
            return NotFound();

        return updatedTask;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoTask? task = await todoService.GetTask(id);
        if (task == null)
            return NotFound();

        bool deleted = await todoService.DeleteTask(id);

        if (!deleted)
            return Conflict();

        return Ok();
    }


}
