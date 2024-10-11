using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public interface IGoalCompletedStatusService
{
	public Task<bool> MarkForUpdate(int goalID);
	public Task<bool> MarkForUpdate(TodoGoal goal);

	public Task<bool> UpdateStatusOfGoalsThatNeeds();
	public Task<bool> UpdateStatusIfGoalNeeds(int goalID);
	public Task<bool> UpdateStatus(int goalID);
	public Task<bool> UpdateStatus(TodoGoal goal);
}
