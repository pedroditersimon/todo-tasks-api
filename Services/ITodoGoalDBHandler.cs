using TodoAPI.Models;

namespace TodoAPI.Services;

public interface ITodoGoalDBHandler
{

    // Get
    public Task<Goal?> GetGoal(int id, bool includeTasks = true);
    public Task<List<Goal>> GetAllGoals(int limit = 50, bool includeTasks = true);
    public Task<List<Goal>> GetPendingGoals(int limit = 50, bool includeTasks = true);
    public Task<List<Goal>> GetCompletedGoals(int limit = 50, bool includeTasks = true);

    // Create
    public Task<Goal?> CreateGoal(Goal goal);

    // Delete
    public Task<bool> DeleteGoal(int id);

    // Update
    public Task<Goal?> UpdateGoal(Goal goal);

}
