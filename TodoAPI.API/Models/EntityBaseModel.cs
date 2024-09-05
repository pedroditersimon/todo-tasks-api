using System.ComponentModel.DataAnnotations;

namespace TodoAPI.API.Models;

public class EntityBaseModel<Tid>
{
    public Tid ID { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }

    [ConcurrencyCheck]
    public DateTime LastUpdatedTime { get; set; }
}
