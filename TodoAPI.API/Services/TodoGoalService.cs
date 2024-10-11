using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Interfaces;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Events;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoGoalService : GenericService<TodoGoal, int>, ITodoGoalService, ISaveable
{
	readonly ITodoGoalRepository _repository;
	readonly ITodoTaskGoalService _taskGoalService;
	readonly IGoalCompletedStatusService _goalCompletedStatusService;

	public event AsyncEventHandler<EventArgs> OnSaveChangesRequested;

	public TodoGoalService(ITodoGoalRepository repository, ITodoTaskGoalService taskGoalService,
		IGoalCompletedStatusService goalCompletedStatusService)
		: base(repository)
	{
		_repository = repository;
		_taskGoalService = taskGoalService;
		_goalCompletedStatusService = goalCompletedStatusService;
	}

	#region Get

	public override async Task<TodoGoal?> GetByID(int id)
	{
		// before do get all, update goals status
		bool updated = await _goalCompletedStatusService.UpdateStatusIfGoalNeeds(id);
		if (updated)
			if (updated) await OnSaveChangesRequested(this, EventArgs.Empty);

		return await base.GetByID(id);
	}

	public new async Task<List<TodoGoal>> GetAll(int limit = 0)
	{
		// before do get all, update goals status
		bool updated = await _goalCompletedStatusService.UpdateStatusOfGoalsThatNeeds();
		if (updated)
			await OnSaveChangesRequested(this, EventArgs.Empty);

		return await base.GetAll(limit).ToListAsync();
	}


	public async Task<List<TodoGoal>> GetPendings(int limit = 0)
	{
		// before do get all, update goals status
		bool updated = await _goalCompletedStatusService.UpdateStatusOfGoalsThatNeeds();
		if (updated)
			await OnSaveChangesRequested(this, EventArgs.Empty);

		return await _repository.GetAll()
			.Where((g) => !g.IsCompleted)
			.OrderBy(g => g.ID)
			.TakeLimit(limit).ToListAsync();
	}

	public async Task<List<TodoGoal>> GetCompleteds(int limit = 0)
	{
		// before do get all, update goals status
		bool updated = await _goalCompletedStatusService.UpdateStatusOfGoalsThatNeeds();
		if (updated)
			await OnSaveChangesRequested(this, EventArgs.Empty);

		return await _repository.GetAll()
			.Where((g) => g.IsCompleted)
			.OrderBy(g => g.ID)
			.TakeLimit(limit).ToListAsync();
	}

	public async Task<List<TodoGoal>> GetAllByTask(int taskID, int limit = 0)
	{
		// before do get all, update goals status
		bool updated = await _goalCompletedStatusService.UpdateStatusOfGoalsThatNeeds();
		if (updated)
			await OnSaveChangesRequested(this, EventArgs.Empty);

		return await _taskGoalService.GetGoalsByTaskID(taskID, limit).ToListAsync();
	}
	#endregion

	#region Update
	public async Task<bool> AddTask(int goalID, int taskID)
	{
		bool success = await _taskGoalService.Associate(goalID, taskID);
		if (!success)
			return false;

		return true;
	}

	public async Task<bool> RemoveTask(int goalID, int taskID)
	{
		bool success = await _taskGoalService.Dissociate(goalID, taskID);
		if (!success)
			return false;

		return true;
	}

	#endregion

}
