using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Services;
using TodoAPI.API.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoGoalController(IUnitOfWork unitOfWork) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<TodoGoal>>> GetAll()
        => await unitOfWork.GoalRepository.GetAll().ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoGoal>> GetByID(int id)
    {
        TodoGoal? goal = await unitOfWork.GoalRepository.GetByID(id);
        if (goal == null)
            return NotFound();

        return goal;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoGoal>>> GetPendings()
        => await unitOfWork.GoalRepository.GetPendings().ToListAsync();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoGoal>>> GetCompleteds()
        => await unitOfWork.GoalRepository.GetCompleteds().ToListAsync();

    [HttpPost]
    public async Task<ActionResult<TodoGoal?>> Create(TodoGoal goal)
    {
        TodoGoal? createdGoal = await unitOfWork.GoalRepository.Create(goal);
        if (createdGoal == null)
            return Conflict();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return createdGoal;
    }


    [HttpPut]
    public async Task<ActionResult<TodoGoal>> Update(TodoGoal goal)
    {
        TodoGoal? updatedGoal = await unitOfWork.GoalRepository.Update(goal);
        if (updatedGoal == null)
            return NotFound();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return updatedGoal;
    }

    [HttpPut(nameof(AddTask))]
    public async Task<ActionResult<TodoGoal>> AddTask(int goalID, int taskID)
    {
        TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
        if (task == null)
            return NotFound();

        bool success = await unitOfWork.GoalRepository.AddTask(goalID, task);
        if (!success)
            return Conflict();

        // save
        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        // get updated entity
        TodoGoal? goal = await unitOfWork.GoalRepository.GetByIDWithTasks(goalID);
        if (goal == null)
            return NotFound();

        return goal;
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoGoal? goal = await unitOfWork.GoalRepository.GetByID(id);
        if (goal == null)
            return NotFound();

        bool deleted = await unitOfWork.GoalRepository.SoftDelete(id);

        if (!deleted)
            return Conflict();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return NoContent();
    }


}
