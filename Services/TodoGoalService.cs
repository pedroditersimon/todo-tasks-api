using TodoAPI.Models;

namespace TodoAPI.Services;

public class TodoGoalService(ITodoGoalDBHandler goalDbHandler) : ITodoGoalDBHandler
{
    readonly ITodoGoalDBHandler goalDbHandler = goalDbHandler;

    public Task<Goal?> CreateGoal(Goal goal)
    {
        return goalDbHandler.CreateGoal(goal);
    }



    public Task<bool> DeleteGoal(int id)
    {
        return goalDbHandler.DeleteGoal(id);
    }



    public Task<List<Goal>> GetAllGoals(int limit = 50, bool includeTasks = true)
    {
        return goalDbHandler.GetAllGoals(limit, includeTasks);
    }


    public Task<List<Goal>> GetCompletedGoals(int limit = 50, bool includeTasks = true)
    {
        return goalDbHandler.GetCompletedGoals(limit, includeTasks);
    }


    public Task<Goal?> GetGoal(int id, bool includeTasks = true)
    {
        return goalDbHandler.GetGoal(id, includeTasks);
    }

    public Task<List<Goal>> GetPendingGoals(int limit = 50, bool includeTasks = true)
    {
        return goalDbHandler.GetPendingGoals(limit, includeTasks);
    }

    public Task<Goal?> UpdateGoal(Goal goal)
    {
        return goalDbHandler.UpdateGoal(goal);
    }

}
