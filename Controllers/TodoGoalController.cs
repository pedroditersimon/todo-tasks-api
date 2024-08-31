using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoGoalController(IUnitOfWork unitOfWork) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<TodoGoal>>> GetAll()
        => await unitOfWork.GoalRepository.GetAllGoals();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoGoal>> GetByID(int id)
    {
        TodoGoal? goal = await unitOfWork.GoalRepository.GetGoal(id);
        if (goal == null)
            return NotFound();

        return goal;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoGoal>>> GetPendings()
        => await unitOfWork.GoalRepository.GetPendingGoals();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoGoal>>> GetCompleteds()
        => await unitOfWork.GoalRepository.GetCompletedGoals();

    [HttpPost]
    public async Task<ActionResult<TodoGoal?>> Create(TodoGoal goal)
    {
        TodoGoal? createdGoal = await unitOfWork.GoalRepository.CreateGoal(goal);
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
        TodoGoal? updatedGoal = await unitOfWork.GoalRepository.UpdateGoal(goal);
        if (updatedGoal == null)
            return NotFound();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return updatedGoal;
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoGoal? goal = await unitOfWork.GoalRepository.GetGoal(id);
        if (goal == null)
            return NotFound();

        bool deleted = await unitOfWork.GoalRepository.DeleteGoal(id);

        if (!deleted)
            return Conflict();

        bool saved = await unitOfWork.Save() > 0;
        if (!saved)
            return Conflict();

        return NoContent();
    }


}
