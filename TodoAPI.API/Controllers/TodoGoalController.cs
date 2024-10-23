using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using TodoAPI.API.Services;
using TodoAPI.Data.DTOs.TodoGoal;
using TodoAPI.Data.Mappers;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Controllers;

[ApiController]
[Route("Goals")]
public class TodoGoalController(IUnitOfWork unitOfWork) : ControllerBase
{
	// Since all retrieval methods modify goals that require an update to their completion status,
	// the HTTP method should be POST instead of GET according to REST principles


	[HttpPost(nameof(GetAll))]
	public async Task<ActionResult<List<GoalResponse>>> GetAll()
	{
		List<TodoGoal> goals = await unitOfWork.GoalService.GetAll();
		return goals.Select(g => g.ToResponse(unitOfWork.Mapper)).ToList();
	}


	[HttpPost(nameof(GetByID) + "/{id}")]
	public async Task<ActionResult<GoalResponse>> GetByID(int id)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(id);
		if (goal == null)
			return NotFound();

		return goal.ToResponse(unitOfWork.Mapper);
	}

	[HttpPost(nameof(GetAllByTaskID) + "/{taskID}")]
	public async Task<ActionResult<List<GoalResponse>>> GetAllByTaskID(int taskID)
	{
		TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
		if (task == null)
			return NotFound();

		List<TodoGoal> goals = await unitOfWork.GoalService.GetAllByTask(taskID);
		return goals.Select(g => g.ToResponse(unitOfWork.Mapper)).ToList();
	}

	[HttpPost(nameof(GetPendings))]
	public async Task<ActionResult<List<GoalResponse>>> GetPendings()
	{
		List<TodoGoal> goals = await unitOfWork.GoalService.GetPendings();
		return goals.Select(g => g.ToResponse(unitOfWork.Mapper)).ToList();
	}

	[HttpPost(nameof(GetCompleteds))]
	public async Task<ActionResult<List<GoalResponse>>> GetCompleteds()
	{
		List<TodoGoal> goals = await unitOfWork.GoalService.GetCompleteds();
		return goals.Select(g => g.ToResponse(unitOfWork.Mapper)).ToList();
	}

	[HttpPost(nameof(Create))]
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


	[HttpPut(nameof(Update))]
	public async Task<ActionResult<GoalResponse>> Update(UpdateGoalRequest updateGoalRequest)
	{
		TodoGoal? originalGoal = await unitOfWork.GoalService.GetByID(updateGoalRequest.ID);
		if (originalGoal == null)
			return NotFound();

		TodoGoal goal = originalGoal.ReplaceWith(updateGoalRequest);

		TodoGoal? updatedGoal = await unitOfWork.GoalService.Update(goal);
		if (updatedGoal == null)
			return NotFound();

		bool saved = await unitOfWork.Save() > 0;
		if (!saved)
			return Conflict();

		return updatedGoal.ToResponse(unitOfWork.Mapper);
	}


	[HttpPatch(nameof(AddTask))]
	public async Task<ActionResult<GoalResponse>> AddTask(int goalID, ImmutableArray<int> taskIDs)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(goalID);
		if (goal == null)
			return NotFound();

		// add every id
		foreach (var taskID in taskIDs)
		{
			TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
			if (task == null)
				return NotFound();

			bool success = await unitOfWork.GoalService.AddTask(goalID, taskID);
			if (!success)
				return Conflict();
		}

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
	public async Task<ActionResult<GoalResponse>> RemoveTask(int goalID, ImmutableArray<int> taskIDs)
	{
		TodoGoal? goal = await unitOfWork.GoalService.GetByID(goalID);
		if (goal == null)
			return NotFound();

		// remove every id
		foreach (var taskID in taskIDs)
		{
			TodoTask? task = await unitOfWork.TaskRepository.GetByID(taskID);
			if (task == null)
				return NotFound();

			bool successRemoved = await unitOfWork.GoalService.RemoveTask(goalID, taskID);
			if (!successRemoved)
				return Conflict();
		}

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
