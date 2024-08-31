using TodoAPI.Models;

namespace TodoAPI.Repositories;

public interface ITodoGoalRepository
{

    // Get
    public Task<TodoGoal?> GetGoal(int id, bool includeTasks = true);
    public Task<List<TodoGoal>> GetAllGoals(int limit = 50, bool includeTasks = true);
    public Task<List<TodoGoal>> GetPendingGoals(int limit = 50, bool includeTasks = true);
    public Task<List<TodoGoal>> GetCompletedGoals(int limit = 50, bool includeTasks = true);

    // Create
    public Task<TodoGoal?> CreateGoal(TodoGoal goal);

    // Delete
    public Task<bool> DeleteGoal(int id);

    // Update
    public Task<TodoGoal?> UpdateGoal(TodoGoal goal);

}
