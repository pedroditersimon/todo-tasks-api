
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Data.Models;

public class TodoTask : EntityBaseModel<int>
{

	public string? Name { get; set; }
	public string? Description { get; set; }

	public bool IsCompleted { get; set; }
	public bool IsFavorite { get; set; }


	[Column("TodoGoalID")]
	public int? GoalID { get; set; }
	[ForeignKey("GoalID")]
	public TodoGoal? Goal { get; set; }
}
