using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoGoalController(TodoGoalService goalService) : ControllerBase
{

    readonly TodoGoalService goalService = goalService;

    [HttpGet]
    public async Task<ActionResult<List<Goal>>> GetAll()
        => await goalService.GetAllGoals();

    [HttpGet("{id}")]
    public async Task<ActionResult<Goal>> GetByID(int id)
    {
        Goal? goal = await goalService.GetGoal(id);
        if (goal == null)
            return NotFound();

        return goal;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<Goal>>> GetPendings()
        => await goalService.GetPendingGoals();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<Goal>>> GetCompleteds()
        => await goalService.GetCompletedGoals();

    [HttpPost]
    public async Task<ActionResult<Goal?>> Create(Goal goal)
        => await goalService.CreateGoal(goal);

    [HttpPut]
    public async Task<ActionResult<Goal>> Update(Goal goal)
    {
        Goal? updatedGoal = await goalService.UpdateGoal(goal);
        if (updatedGoal == null)
            return NotFound();

        return updatedGoal;
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        Goal? goal = await goalService.GetGoal(id);
        if (goal == null)
            return NotFound();

        bool deleted = await goalService.DeleteGoal(id);

        if (!deleted)
            return Conflict();

        return Ok();
    }


}
