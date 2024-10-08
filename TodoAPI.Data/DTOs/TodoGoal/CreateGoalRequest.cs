namespace TodoAPI.Data.DTOs.TodoGoal;

public class CreateGoalRequest
{
	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsFavorite { get; set; }
}
