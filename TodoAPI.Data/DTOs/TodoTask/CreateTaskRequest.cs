namespace TodoAPI.Data.DTOs.TodoTask;

public class CreateTaskRequest
{
	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsCompleted { get; set; }
	public bool IsFavorite { get; set; }
}
