namespace TodoAPI.Data.DTOs.TodoTask;

public class TaskResponse
{
	// base model
	public int ID { get; set; }

	public DateTime CreationDate { get; set; }
	public DateTime LastUpdatedTime { get; set; }


	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsCompleted { get; set; }
	public bool IsFavorite { get; set; }
}
