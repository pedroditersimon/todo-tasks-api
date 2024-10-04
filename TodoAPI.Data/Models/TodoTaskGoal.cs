namespace TodoAPI.Data.Models;

// relational model
public class TodoTaskGoal
{
	public int ID { get; set; }

	public int TodoTaskID { get; set; }
	public TodoTask TodoTask { get; set; }


	public int TodoGoalID { get; set; }
	public TodoGoal TodoGoal { get; set; }
}
