using TodoAPI.API.Services;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Repositories;

public class TodoGoalRepository(TodoDBContext dbContext)
	: GenericRepository<TodoGoal, int>(dbContext), ITodoGoalRepository
{


}
