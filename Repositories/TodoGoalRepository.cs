using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Repositories;

public class TodoGoalRepository(TodoDBContext dbContext) : ITodoGoalRepository
{

    // Get
    public async Task<TodoGoal?> GetGoal(int id, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? dbContext.Goals.Include(g => g.Tasks) : dbContext.Goals;

        return await query.SingleOrDefaultAsync(g => g.ID == id);
    }

    public async Task<List<TodoGoal>> GetAllGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? dbContext.Goals.Include(g => g.Tasks) : dbContext.Goals;

        return query.OrderBy(g => g.ID).Take(limit).ToList();
    }

    public async Task<List<TodoGoal>> GetPendingGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? dbContext.Goals.Include(g => g.Tasks) : dbContext.Goals;

        return query.Where((g) => g.Tasks.Any(task => !task.Completed)).OrderBy(g => g.ID).Take(limit).ToList();
    }

    public async Task<List<TodoGoal>> GetCompletedGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? dbContext.Goals.Include(g => g.Tasks) : dbContext.Goals;

        return query.Where((g) => !g.Tasks.Any(task => !task.Completed)).OrderBy(g => g.ID).Take(limit).ToList();
    }

    // Create
    public async Task<TodoGoal?> CreateGoal(TodoGoal goal)
    {
        EntityEntry<TodoGoal> entry = dbContext.Goals.Add(goal);
        return entry.Entity;
    }

    // Delete
    public async Task<bool> DeleteGoal(int id)
    {
        TodoGoal? goal = await GetGoal(id);
        if (goal == null)
            return false;

        dbContext.Tasks.RemoveRange(goal.Tasks);
        dbContext.Goals.Remove(goal);
        return true;
    }

    // Update
    public async Task<TodoGoal?> UpdateGoal(TodoGoal goal)
    {
        TodoGoal? currentGoal = await GetGoal(goal.ID);
        if (currentGoal == null)
            return null;

        dbContext.Entry(currentGoal).CurrentValues.SetValues(goal);
        return currentGoal;
    }

}
