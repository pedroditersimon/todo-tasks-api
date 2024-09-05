using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoTaskController(IUnitOfWork unitOfWork) : ControllerBase
{


    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> GetAll()
        => await unitOfWork.TaskRepository.GetAll().ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetByID(int id)
    {
        TodoTask? task = await unitOfWork.TaskRepository.GetByID(id);
        if (task == null)
            return NotFound();

        return task;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoTask>>> GetPendings()
        => await unitOfWork.TaskRepository.GetPendings().ToListAsync();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoTask>>> GetCompleteds()
        => await unitOfWork.TaskRepository.GetCompleteds().ToListAsync();

    [HttpPost]
    public async Task<ActionResult<TodoTask?>> Create(TodoTask task)
    {
        TodoTask? createdTask = await unitOfWork.TaskRepository.Create(task);
        if (createdTask == null)
            return Conflict();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return createdTask;
    }


    [HttpPut]
    public async Task<ActionResult<TodoTask>> Update(TodoTask task)
    {
        TodoTask? updatedTask = await unitOfWork.TaskRepository.Update(task);
        if (updatedTask == null)
            return NotFound();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return updatedTask;
    }

    [HttpPut(nameof(SetCompleted))]
    public async Task<ActionResult<TodoTask>> SetCompleted(int id, bool completed)
    {
        TodoTask? updatedTask = await unitOfWork.TaskRepository.SetCompleted(id, completed);
        if (updatedTask == null)
            return NotFound();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return updatedTask;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoTask? task = await unitOfWork.TaskRepository.GetByID(id);
        if (task == null)
            return NotFound();

        bool deleted = await unitOfWork.TaskRepository.SoftDelete(id);

        if (!deleted)
            return Conflict();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return NoContent();
    }


}
