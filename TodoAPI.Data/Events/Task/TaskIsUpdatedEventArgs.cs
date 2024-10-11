namespace TodoAPI.Data.Events.Task;

public class TaskIsUpdatedEventArgs : EventArgs
{
    public int TaskID { get; }
    public bool IsCompleted { get; }

    public TaskIsUpdatedEventArgs(int taskID, bool isCompleted)
    {
        TaskID = taskID;
        IsCompleted = isCompleted;
    }
}
