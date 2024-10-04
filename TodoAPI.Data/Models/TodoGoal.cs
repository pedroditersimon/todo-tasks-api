namespace TodoAPI.Data.Models;

public class TodoGoal : EntityBaseModel<int>
{
	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsCompleted { get; set; }
	public bool IsFavorite { get; set; }

	public ICollection<TodoTaskGoal> TodoTaskGoal { get; set; }
}
