using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EntityLoadingTest(TodoDBContext dbContext) : ControllerBase
{

    [HttpGet(nameof(EagerLoading))]
    public async Task<ActionResult<TodoGoal?>> EagerLoading(int goalID = 1)
    {
        return dbContext.Goals
            .Include(g => g.Tasks)
            .SingleOrDefault(g => g.ID == goalID);
    }

    [HttpGet(nameof(LazyLoading))]
    public async Task<ActionResult<TodoGoal>> LazyLoading(int goalID = 1)
    {
        TodoGoal? goal = dbContext.Goals.SingleOrDefault(g => g.ID == goalID);
        if (goal == null)
            return NotFound();

        // must have: UseLazyLoadingProxies() from Microsoft.EntityFrameworkCore.Proxies
        ICollection<TodoTask> goalTasks = goal.Tasks.ToList();

        return goal;
    }


    [HttpGet(nameof(ExplicitLoading))]
    public async Task<ActionResult<TodoGoal>> ExplicitLoading(int goalID = 1)
    {
        TodoGoal? goal = dbContext.Goals.SingleOrDefault(g => g.ID == goalID);
        if (goal == null)
            return NotFound();

        // this doesnt require UseLazyLoadingProxies and can bypass it
        await dbContext.Entry(goal)
            .Collection(g => g.Tasks)
            .LoadAsync();

        return goal;
    }
}
