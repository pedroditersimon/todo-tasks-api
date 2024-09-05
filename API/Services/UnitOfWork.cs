namespace TodoAPI.Services;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoAPI.Repositories;

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
