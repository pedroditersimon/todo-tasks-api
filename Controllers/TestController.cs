﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Controllers;

[ApiController]
[Route("Test")]
public class TestController(DbContext dbContext, IUnitOfWork unitOfWork) : ControllerBase
{
    #region EntityLoadingTest
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
    #endregion


    #region RawSQL
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
    #endregion

    [HttpGet("FirstLevelCache/GetById")]
    public async Task<ActionResult<TodoTask?>> FirstLevelCache_GetById(int id)
    {
        // You may notice that the second query takes less than 1 ms. (enable ef Logging in appsettings)
        TodoTask? task = await unitOfWork.TaskRepository.GetByID(id);
        TodoTask? task2 = await unitOfWork.TaskRepository.GetByID(id);
        if (task == null || task2 == null)
            return NotFound();

        return task;
    }
}

