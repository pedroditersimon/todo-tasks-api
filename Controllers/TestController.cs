using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("Test")]
public class TestController(DbContext dbContext, IUnitOfWork unitOfWork) : ControllerBase
{

    [HttpGet("EntityLoadingTest/" + nameof(EagerLoading))]
    public async Task<ActionResult<TodoGoal?>> EagerLoading(int goalID = 1)
    {
        return unitOfWork.GoalRepository.GetAll()
            .Include(g => g.Tasks)
            .SingleOrDefault(g => g.ID == goalID);
    }

    [HttpGet("EntityLoadingTest/" + nameof(LazyLoading))]
    public async Task<ActionResult<TodoGoal>> LazyLoading(int goalID = 1)
    {
        TodoGoal? goal = unitOfWork.GoalRepository.GetAll().SingleOrDefault(g => g.ID == goalID);
        if (goal == null)
            return NotFound();

        // must have: UseLazyLoadingProxies() from Microsoft.EntityFrameworkCore.Proxies
        ICollection<TodoTask> goalTasks = goal.Tasks.ToList();

        return goal;
    }


    [HttpGet("EntityLoadingTest/" + nameof(ExplicitLoading))]
    public async Task<ActionResult<TodoGoal>> ExplicitLoading(int goalID = 1)
    {
        TodoGoal? goal = unitOfWork.GoalRepository.GetAll().SingleOrDefault(g => g.ID == goalID);
        if (goal == null)
            return NotFound();

        // this doesnt require UseLazyLoadingProxies and can bypass it
        await dbContext.Entry(goal)
            .Collection(g => g.Tasks)
            .LoadAsync();

        return goal;
    }


    [HttpGet("RawSQL/GetById")]
    public async Task<ActionResult<TodoTask?>> RawSQL_GetById(int id)
    {
        TodoTask? task = await unitOfWork.TaskRepository.RawSQL_GetById(id);
        if (task == null)
            return NotFound();

        return task;
    }


    [HttpGet("RawSQLWithDBSet/GetById")]
    public async Task<ActionResult<TodoTask?>> RawSQLWithDBSet_GetById(int id)
    {
        TodoTask? task = await unitOfWork.TaskRepository.RawSQLWithDBSet_GetById(id);
        if (task == null)
            return NotFound();

        return task;
    }

}

