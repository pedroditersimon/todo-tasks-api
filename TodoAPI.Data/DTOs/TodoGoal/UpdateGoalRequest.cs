namespace TodoAPI.Data.DTOs.TodoGoal;

public class UpdateGoalRequest
{
	public int ID { get; set; }

	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsFavorite { get; set; }
}
