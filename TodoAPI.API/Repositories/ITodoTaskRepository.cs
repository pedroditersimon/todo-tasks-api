using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public interface ITodoTaskRepository : IGenericRepository<TodoTask, int>
{

}
