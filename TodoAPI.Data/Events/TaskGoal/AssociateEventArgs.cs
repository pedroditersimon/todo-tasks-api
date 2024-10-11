namespace TodoAPI.Data.Events.TaskGoal;

public class AssociateEventArgs
{
	public int GoalID { get; }
	public int TaskID { get; }

	public AssociateEventArgs(int goalID, int taskID)
	{
		GoalID = goalID;
		TaskID = taskID;
	}
}
