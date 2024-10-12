using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Data.Models;

public class EntityBaseModel<Tid> : ICloneable
{
	public Tid ID { get; set; }

	public DateTime CreationDate { get; set; }

	public bool IsDeleted { get; set; }
	public DateTime LastDeletedTime { get; set; }

	[ConcurrencyCheck]
	public DateTime LastUpdatedTime { get; set; }

	public virtual object Clone()
		=> MemberwiseClone();
}
