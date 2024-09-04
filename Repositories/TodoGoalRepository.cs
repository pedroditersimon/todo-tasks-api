using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Repositories;

public class TodoGoalRepository(TodoDBContext dbContext)
    : GenericRepository<TodoGoal, int>(dbContext), ITodoGoalRepository
{

    #region Get
    public async Task<TodoGoal?> GetByIDWithTasks(int id)
        => Entities
            .Include(g => g.Tasks)
            .SingleOrDefault(g => g.ID.Equals(id));

    public IQueryable<TodoGoal> GetAllWithTasks(int limit = 50)
        => Entities.Include(g => g.Tasks);

    public IQueryable<TodoGoal> GetPendings(int limit = 50)
        => Entities
            .Where((g) => g.Tasks.Any(task => !task.IsCompleted))
            .OrderBy(g => g.ID)
            .Take(limit);


    public IQueryable<TodoGoal> GetCompleteds(int limit = 50)
        => Entities
            .Where((g) => !g.Tasks.Any(task => !task.IsCompleted))
            .OrderBy(g => g.ID)
            .Take(limit);
    #endregion

    #region Update
    public async Task<bool> AddTask(int goalID, TodoTask task)
    {
        TodoGoal? goal = await GetByIDWithTasks(goalID);
        if (goal == null)
            return false;

        goal.Tasks.Add(task);
        return true;
    }
    #endregion
}
