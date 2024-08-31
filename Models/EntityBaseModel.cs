namespace TodoAPI.Models;

public class EntityBaseModel
{
    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}
