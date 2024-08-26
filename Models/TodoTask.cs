using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Models;

public class TodoTask : ICloneable
{
    [Column("id")]
    public int ID { get; set; }

    [Column("name")]
    public string? Name { get; set; }
    [Column("description")]
    public string? Description { get; set; }

    [Column("completed")]
    public bool Completed { get; set; }

    public object Clone() => this.MemberwiseClone();

}
