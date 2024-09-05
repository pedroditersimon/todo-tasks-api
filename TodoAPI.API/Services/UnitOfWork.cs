
using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public class UnitOfWork(TodoDBContext dbContext, ITodoTaskRepository taskRepository, ITodoGoalRepository goalRepository) : IUnitOfWork
{

    public ITodoTaskRepository TaskRepository { get; } = taskRepository;

    public ITodoGoalRepository GoalRepository { get; } = goalRepository;

    public async Task<int> Save()
    {
        try
        {
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("DbUpdateConcurrencyException");
        }
        return 0;
    }


    public void Dispose()
        => dbContext.Dispose();

}
