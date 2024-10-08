namespace TodoAPI.Data.DTOs.TodoGoal;

public class GoalResponse
{
	public int ID { get; set; }


	public DateTime CreationDate { get; set; }
	public DateTime LastUpdatedTime { get; set; }


	public string? Name { get; set; }
	public string? Description { get; set; }


	// calculated field
	public bool IsCompleted { get; set; }
	public float CompletedPercent { get; set; } // in 0 to 100 range

	public bool IsFavorite { get; set; }
}
