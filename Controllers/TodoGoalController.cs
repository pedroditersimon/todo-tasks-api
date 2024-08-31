using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoGoalController(ITodoGoalRepository goalRepository) : ControllerBase
{

    readonly ITodoGoalRepository goalRepository = goalRepository;

    [HttpGet]
    public async Task<ActionResult<List<TodoGoal>>> GetAll()
        => await goalRepository.GetAllGoals();

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoGoal>> GetByID(int id)
    {
        TodoGoal? goal = await goalRepository.GetGoal(id);
        if (goal == null)
            return NotFound();

        return goal;
    }

    [HttpGet(nameof(GetPendings))]
    public async Task<ActionResult<List<TodoGoal>>> GetPendings()
        => await goalRepository.GetPendingGoals();

    [HttpGet(nameof(GetCompleteds))]
    public async Task<ActionResult<List<TodoGoal>>> GetCompleteds()
        => await goalRepository.GetCompletedGoals();

    [HttpPost]
    public async Task<ActionResult<TodoGoal?>> Create(TodoGoal goal)
        => await goalRepository.CreateGoal(goal);

    [HttpPut]
    public async Task<ActionResult<TodoGoal>> Update(TodoGoal goal)
    {
        TodoGoal? updatedGoal = await goalRepository.UpdateGoal(goal);
        if (updatedGoal == null)
            return NotFound();

        return updatedGoal;
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        TodoGoal? goal = await goalRepository.GetGoal(id);
        if (goal == null)
            return NotFound();

        bool deleted = await goalRepository.DeleteGoal(id);

        if (!deleted)
            return Conflict();

        return Ok();
    }


}
