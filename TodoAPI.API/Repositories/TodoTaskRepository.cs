using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoTaskRepository(TodoDBContext dbContext)
	: GenericRepository<TodoTask, int>(dbContext), ITodoTaskRepository
{

}
