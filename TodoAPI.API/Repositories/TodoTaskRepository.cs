using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoTaskRepository(TodoDBContext dbContext)
	: GenericRepository<TodoTask, int>(dbContext), ITodoTaskRepository
{
	public override IQueryable<TodoTask> GetAll()
	{
		return base.GetAll()
			.OrderBy(t => t.ID)
			.OrderByDescending(t => t.IsCompleted)
			.OrderByDescending(t => t.IsFavorite);
	}
}
