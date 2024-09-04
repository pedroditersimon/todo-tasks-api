namespace TodoAPI.Models;

public class TodoTask : EntityBaseModel<int>
{

    public string? Name { get; set; }
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

}
