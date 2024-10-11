namespace TodoAPI.Data.Events.Task;

public class TaskIsDeletedEventArgs : EventArgs
{
    public int TaskID { get; }

    public TaskIsDeletedEventArgs(int taskID)
    {
        TaskID = taskID;
    }
}
