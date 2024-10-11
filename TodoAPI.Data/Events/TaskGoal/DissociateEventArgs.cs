namespace TodoAPI.Data.Events.TaskGoal;

public class DissociateEventArgs
{
	public int GoalID { get; }
	public int TaskID { get; }

	public DissociateEventArgs(int goalID, int taskID)
	{
		GoalID = goalID;
		TaskID = taskID;
	}
}
