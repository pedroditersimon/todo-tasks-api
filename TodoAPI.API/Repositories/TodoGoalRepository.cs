using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoGoalRepository(TodoDBContext dbContext)
	: GenericRepository<TodoGoal, int>(dbContext), ITodoGoalRepository
{

	public override IQueryable<TodoGoal> GetAll()
	{
		return base.GetAll()
			.OrderByDescending(g => g.IsFavorite)
			.ThenByDescending(g => g.IsCompleted)
			.ThenBy(g => g.ID);
	}
}
