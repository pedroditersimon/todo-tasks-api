using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoGoalRepository(TodoDBContext dbContext)
	: GenericRepository<TodoGoal, int>(dbContext), ITodoGoalRepository
{

	public override IQueryable<TodoGoal> GetAll()
	{
		return base.GetAll()
			.OrderBy(g => g.ID)
			.OrderByDescending(g => g.IsCompleted)
			.OrderByDescending(g => g.IsFavorite);
	}
}
