using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Services;
using TodoAPI.Data.DTOs.TodoGoal;
using TodoAPI.Data.Mappers;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("Goals")]
public class TodoGoalController(IUnitOfWork unitOfWork) : ControllerBase
{

	[HttpGet]
	public async Task<ActionResult<List<GoalResponse>>> GetAll()
		=> await unitOfWork.GoalService.GetAll()
			.Select(g => g.ToResponse(unitOfWork.Mapper)).ToListAsync();


	[HttpGet("{id}")]
	public async Task<ActionResult<GoalResponse>> GetByID(int id)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(id);
		if (goal == null)
			return NotFound();

		return goal.ToResponse(unitOfWork.Mapper);
	}

	[HttpGet(nameof(GetAllByTask))]
	public async Task<ActionResult<List<GoalResponse>>> GetAllByTask(int taskID)
	{
		TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
		if (task == null)
			return NotFound();

		return await unitOfWork.GoalService.GetAllByTask(taskID)
			.Select(g => g.ToResponse(unitOfWork.Mapper)).ToListAsync();
	}

	[HttpGet(nameof(GetPendings))]
	public async Task<ActionResult<List<GoalResponse>>> GetPendings()
		=> await unitOfWork.GoalService.GetPendings()
			.Select(g => g.ToResponse(unitOfWork.Mapper)).ToListAsync();

	[HttpGet(nameof(GetCompleteds))]
	public async Task<ActionResult<List<GoalResponse>>> GetCompleteds()
		=> await unitOfWork.GoalService.GetCompleteds()
			.Select(g => g.ToResponse(unitOfWork.Mapper)).ToListAsync();

	[HttpPost]
	public async Task<ActionResult<GoalResponse?>> Create(CreateGoalRequest createGoalRequest)
	{
		TodoGoal goal = createGoalRequest.ToGoal(unitOfWork.Mapper);

		TodoGoal? createdGoal = await unitOfWork.GoalService.Create(goal);
		if (createdGoal == null)
			return Conflict();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return createdGoal.ToResponse(unitOfWork.Mapper);
	}


	[HttpPut]
	public async Task<ActionResult<GoalResponse>> Update(UpdateGoalRequest updateGoalRequest)
	{
		TodoGoal goal = updateGoalRequest.ToGoal(unitOfWork.Mapper);

		TodoGoal? updatedGoal = await unitOfWork.GoalService.Update(goal);
		if (updatedGoal == null)
			return NotFound();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return updatedGoal.ToResponse(unitOfWork.Mapper);
	}

	[HttpPatch(nameof(AddTask))]
	public async Task<ActionResult<GoalResponse>> AddTask(int goalID, int taskID)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(goalID);
		if (goal == null)
			return NotFound();

		TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
		if (task == null)
			return NotFound();

		bool success = await unitOfWork.GoalService.AddTask(goalID, taskID);
		if (!success)
			return Conflict();

		// save
		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		// get updated entity
		TodoGoal? updatedGoal = await unitOfWork.GoalService.GetByID(goalID);
		if (updatedGoal == null)
			return Conflict();

		return updatedGoal.ToResponse(unitOfWork.Mapper);
	}

	[HttpPatch(nameof(RemoveTask))]
	public async Task<ActionResult<GoalResponse>> RemoveTask(int goalID, int taskID)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(goalID);
		if (goal == null)
			return NotFound();

		TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
		if (task == null)
			return NotFound();

		bool successRemoved = await unitOfWork.GoalService.RemoveTask(goalID, taskID);
		if (!successRemoved)
			return Conflict();

		// save
		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		// get updated entity
		TodoGoal? updatedGoal = await unitOfWork.GoalService.GetByID(goalID);
		if (updatedGoal == null)
			return Conflict();

		return updatedGoal.ToResponse(unitOfWork.Mapper);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(int id)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(id);
		if (goal == null)
			return NotFound();

		bool deleted = await unitOfWork.GoalService.SoftDelete(id);

		if (!deleted)
			return Conflict();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return NoContent();
	}


}
