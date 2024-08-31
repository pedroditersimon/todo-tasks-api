namespace TodoAPI.Models;

public class TodoGoal : ICloneable
{
    public int ID { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<TodoTask> Tasks { get; set; }

    public object Clone() => this.MemberwiseClone();
}
