using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("Tasks")]
public class TodoTaskController(IUnitOfWork unitOfWork) : ControllerBase
{


	[HttpGet]
	public async Task<ActionResult<List<TodoTask>>> GetAll()
		=> await unitOfWork.TaskService.GetAll().ToListAsync();

	[HttpGet("{id}")]
	public async Task<ActionResult<TodoTask>> GetByID(int id)
	{
		TodoTask? task = await unitOfWork.TaskService.GetByID(id);
		if (task == null)
			return NotFound();

		return task;
	}

	[HttpGet(nameof(GetAllByTask))]
	public async Task<ActionResult<List<TodoTask>>> GetAllByTask(int goalID)
	{
		TodoGoal? goal = await unitOfWork.GoalRepository.GetByID(goalID);
		if (goal == null)
			return NotFound();

		return await unitOfWork.TaskService.GetAllByGoal(goalID).ToListAsync();
	}

	[HttpGet(nameof(GetPendings))]
	public async Task<ActionResult<List<TodoTask>>> GetPendings()
		=> await unitOfWork.TaskService.GetPendings().ToListAsync();

	[HttpGet(nameof(GetCompleteds))]
	public async Task<ActionResult<List<TodoTask>>> GetCompleteds()
		=> await unitOfWork.TaskService.GetCompleteds().ToListAsync();

	[HttpPost]
	public async Task<ActionResult<TodoTask?>> Create(TodoTask task)
	{
		TodoTask? createdTask = await unitOfWork.TaskService.Create(task);
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
		TodoTask? updatedTask = await unitOfWork.TaskService.Update(task);
		if (updatedTask == null)
			return NotFound();

		/*
		bool successUpdatedStatus = await unitOfWork.GoalService.UpdateCompletedStatus(goalID);
		if (!successUpdatedStatus)
			return Conflict();
		*/

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return updatedTask;
	}

	[HttpPut(nameof(SetCompleted))]
	public async Task<ActionResult<TodoTask>> SetCompleted(int id, bool completed)
	{
		TodoTask? updatedTask = await unitOfWork.TaskService.SetCompleted(id, completed);
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
		TodoTask? task = await unitOfWork.TaskService.GetByID(id);
		if (task == null)
			return NotFound();

		bool deleted = await unitOfWork.TaskService.SoftDelete(id);

		if (!deleted)
			return Conflict();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return NoContent();
	}


}
