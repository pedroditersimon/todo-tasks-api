namespace TodoAPI.Models;

public class EntityBaseModel<Tid>
{
    public Tid ID { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}
