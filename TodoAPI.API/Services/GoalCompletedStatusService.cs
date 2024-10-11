using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Events.Task;
using TodoAPI.Data.Events.TaskGoal;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

// The completed status of a goal caches the values, IsCompleted and IsCompletedPercent,
// which reflect the completion progress of its associated tasks.
// The process of calculating these values is deferred until the goal is requested for the first time.
// This approach avoids unnecessary CPU usage that would occur if the status were recalculated 
// every time a task is modified.
// Instead, when a task associated with a goal is updated, the goal is marked as "dirty".
// The next time the goal is requested, the status is recalculated and cached for future use.
// On subsequent requests, the cached status is returned directly without recalculating.
public class GoalCompletedStatusService : IGoalCompletedStatusService
{
	readonly ITodoTaskService _taskService;
	readonly ITodoGoalRepository _goalRepository;
	readonly ITodoTaskGoalService _taskGoalService;

	public GoalCompletedStatusService(ITodoTaskService taskService,
		ITodoGoalRepository goalRepository, ITodoTaskGoalService taskGoalService)
	{
		_taskService = taskService;
		_goalRepository = goalRepository;
		_taskGoalService = taskGoalService;

		_taskService.OnTaskIsDeleted += OnTaskIsDeleted;
		_taskService.OnTaskIsUpdated += OnTaskIsUpdated;

		_taskGoalService.OnAssociate += OnAssociateTaskGoal;
		_taskGoalService.OnDissociate += OnDissociateTaskGoal;
	}


	#region Events
	private async void OnDissociateTaskGoal(object? sender, DissociateEventArgs e)
	{
		bool success = await MarkForUpdate(e.GoalID);
	}

	private async void OnAssociateTaskGoal(object? sender, AssociateEventArgs e)
	{
		bool success = await MarkForUpdate(e.GoalID);
	}

	private async void OnTaskIsDeleted(object? sender, TaskIsDeletedEventArgs e)
	{
		// this will trigger OnDissociateTaskGoal of every goal
		bool success = await _taskGoalService.DissociateAllByTaskID(e.TaskID);
	}

	private async void OnTaskIsUpdated(object? sender, TaskIsUpdatedEventArgs e)
	{
		List<TodoGoal> goals = await _taskGoalService.GetGoalsByTaskID(e.TaskID).ToListAsync();
		foreach (var g in goals)
		{
			bool success = await MarkForUpdate(g.ID);
		}
	}
	#endregion


	#region Mark Goal To Update Status
	public async Task<bool> MarkForUpdate(int goalID)
	{
		TodoGoal? goal = await _goalRepository.GetByID(goalID);
		if (goal == null)
			return false;

		return await MarkForUpdate(goal);
	}

	public async Task<bool> MarkForUpdate(TodoGoal goal)
	{
		// already marked
		if (goal.NeedsToUpdateCompletedStatus)
			return true;

		goal.NeedsToUpdateCompletedStatus = true;

		TodoGoal? updatedGoal = await _goalRepository.Update(goal);
		return updatedGoal != null;
	}
	#endregion


	#region Update Goal Status
	public async Task<bool> UpdateStatusOfGoalsThatNeeds()
	{
		List<TodoGoal> goals = await _goalRepository.GetAll()
			.Where(g => g.NeedsToUpdateCompletedStatus).ToListAsync();

		foreach (var goal in goals)
		{
			bool success = await UpdateStatus(goal);
			if (!success)
				return false;
		}
		return true;
	}

	public async Task<bool> UpdateStatusIfGoalNeeds(int goalID)
	{
		TodoGoal? goal = await _goalRepository.GetByID(goalID);
		if (goal == null)
			return false;

		// no needed
		if (!goal.NeedsToUpdateCompletedStatus)
			return false;

		return await UpdateStatus(goal);
	}

	public async Task<bool> UpdateStatus(int goalID)
	{
		TodoGoal? goal = await _goalRepository.GetByID(goalID);
		if (goal == null)
			return false;

		return await UpdateStatus(goal);
	}

	public async Task<bool> UpdateStatus(TodoGoal goal)
	{
		List<TodoTask> tasks = await _taskGoalService.GetTasksByGoalID(goal.ID).ToListAsync();

		// calculate goal completed percent
		if (tasks.Count > 0)
		{
			int completedTasksCount = tasks.Count(t => t.IsCompleted);
			float percent = completedTasksCount / (float)tasks.Count;
			goal.CompletedPercent = MathF.Truncate(percent * 100);
		}
		else
		{
			goal.CompletedPercent = 0;
		}

		// calculate goal completed status
		goal.IsCompleted = goal.CompletedPercent >= 100.0f;

		// reset update flag
		goal.NeedsToUpdateCompletedStatus = false;

		TodoGoal? updatedGoal = await _goalRepository.Update(goal);
		if (updatedGoal == null)
			return false;

		return true;
	}
	#endregion
}
