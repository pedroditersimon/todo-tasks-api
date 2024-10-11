namespace TodoAPI.Data.Models;

public class TodoGoal : EntityBaseModel<int>
{
	public string? Name { get; set; }
	public string? Description { get; set; }


	// Calculated Fields
	public bool NeedsToUpdateCompletedStatus { get; set; }
	public bool IsCompleted { get; set; }
	public float CompletedPercent { get; set; } // 0 to 100 range


	public bool IsFavorite { get; set; }

	public ICollection<TodoTaskGoal> TodoTaskGoal { get; set; }
}
