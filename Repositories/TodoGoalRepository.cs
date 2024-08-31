using TodoAPI.Models;

namespace TodoAPI.Repositories;

public class TodoGoalRepository(ITodoGoalRepository goalRepository) : ITodoGoalRepository
{
    readonly ITodoGoalRepository goalRepository = goalRepository;

    public Task<TodoGoal?> CreateGoal(TodoGoal goal)
    {
        return goalRepository.CreateGoal(goal);
    }



    public Task<bool> DeleteGoal(int id)
    {
        return goalRepository.DeleteGoal(id);
    }



    public Task<List<TodoGoal>> GetAllGoals(int limit = 50, bool includeTasks = true)
    {
        return goalRepository.GetAllGoals(limit, includeTasks);
    }


    public Task<List<TodoGoal>> GetCompletedGoals(int limit = 50, bool includeTasks = true)
    {
        return goalRepository.GetCompletedGoals(limit, includeTasks);
    }


    public Task<TodoGoal?> GetGoal(int id, bool includeTasks = true)
    {
        return goalRepository.GetGoal(id, includeTasks);
    }

    public Task<List<TodoGoal>> GetPendingGoals(int limit = 50, bool includeTasks = true)
    {
        return goalRepository.GetPendingGoals(limit, includeTasks);
    }

    public Task<TodoGoal?> UpdateGoal(TodoGoal goal)
    {
        return goalRepository.UpdateGoal(goal);
    }

}
