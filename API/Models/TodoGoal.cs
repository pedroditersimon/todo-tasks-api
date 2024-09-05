namespace TodoAPI.Models;

public class TodoGoal : EntityBaseModel<int>
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<TodoTask> Tasks { get; set; }
}
