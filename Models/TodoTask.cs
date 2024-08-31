namespace TodoAPI.Models;

public class TodoTask
{
    public int ID { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public bool Completed { get; set; }

    public DateTime CreationDate { get; set; }


}
