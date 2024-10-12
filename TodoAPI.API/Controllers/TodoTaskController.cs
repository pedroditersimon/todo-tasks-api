using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Services;
using TodoAPI.Data.DTOs.TodoTask;
using TodoAPI.Data.Mappers;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("Tasks")]
public class TodoTaskController(IUnitOfWork unitOfWork) : ControllerBase
{


	[HttpGet]
	public async Task<ActionResult<List<TaskResponse>>> GetAll()
		=> await unitOfWork.TaskService.GetAll()
			.Select(t => t.ToResponse(unitOfWork.Mapper)).ToListAsync();

	[HttpGet("{id}")]
	public async Task<ActionResult<TaskResponse>> GetByID(int id)
	{
		TodoTask? task = await unitOfWork.TaskService.GetByID(id);
		if (task == null)
			return NotFound();

		return task.ToResponse(unitOfWork.Mapper);
	}

	[HttpGet(nameof(GetAllByGoalID) + "/{goalID}")]
	public async Task<ActionResult<List<TaskResponse>>> GetAllByGoalID(int goalID)
	{
		TodoGoal? goal = await unitOfWork.GoalRepository.GetByID(goalID);
		if (goal == null)
			return NotFound();

		return await unitOfWork.TaskService.GetAllByGoal(goalID)
			.Select(t => t.ToResponse(unitOfWork.Mapper)).ToListAsync();
	}

	[HttpGet(nameof(GetPendings))]
	public async Task<ActionResult<List<TaskResponse>>> GetPendings()
		=> await unitOfWork.TaskService.GetPendings()
		.Select(t => t.ToResponse(unitOfWork.Mapper)).ToListAsync();

	[HttpGet(nameof(GetCompleteds))]
	public async Task<ActionResult<List<TaskResponse>>> GetCompleteds()
		=> await unitOfWork.TaskService.GetCompleteds()
		.Select(t => t.ToResponse(unitOfWork.Mapper)).ToListAsync();

	[HttpPost]
	public async Task<ActionResult<TaskResponse?>> Create(CreateTaskRequest createTaskRequest)
	{
		TodoTask task = createTaskRequest.ToTask(unitOfWork.Mapper);

		TodoTask? createdTask = await unitOfWork.TaskService.Create(task);
		if (createdTask == null)
			return Conflict();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return createdTask.ToResponse(unitOfWork.Mapper);
	}


	[HttpPut]
	public async Task<ActionResult<TaskResponse>> Update(UpdateTaskRequest updateTaskRequest)
	{
		TodoTask? originalTask = await unitOfWork.TaskService.GetByID(updateTaskRequest.ID);
		if (originalTask == null)
			return NotFound();

		TodoTask task = originalTask.ReplaceWith(updateTaskRequest);

		TodoTask? updatedTask = await unitOfWork.TaskService.Update(task);
		if (updatedTask == null)
			return NotFound();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return updatedTask.ToResponse(unitOfWork.Mapper);
	}

	[HttpPatch(nameof(SetCompleted))]
	public async Task<ActionResult<TaskResponse>> SetCompleted(int id, bool completed)
	{
		TodoTask? updatedTask = await unitOfWork.TaskService.SetCompleted(id, completed);
		if (updatedTask == null)
			return NotFound();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return updatedTask.ToResponse(unitOfWork.Mapper);
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
