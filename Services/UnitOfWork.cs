namespace TodoAPI.Services;
using System.Threading.Tasks;
using TodoAPI.Repositories;

public class UnitOfWork(TodoDBContext dbContext, ITodoTaskRepository taskRepository, ITodoGoalRepository goalRepository) : IUnitOfWork
{

    public ITodoTaskRepository TaskRepository { get; } = taskRepository;

    public ITodoGoalRepository GoalRepository { get; } = goalRepository;

    public async Task<int> Save()
        => await dbContext.SaveChangesAsync();

    public void Dispose()
        => dbContext.Dispose();

}
