using System.ComponentModel.DataAnnotations;
using Server.Db.Models.Types;

namespace Server.Db.Models.Base;

public class BaseModel
{
    [Required] public int Id { get; set; }

    [Required] public ItemState State { get; set; } = ItemState.Activated;

    [Required] public DateTime Created { get; set; } = DateTime.UtcNow;

    [Required] public DateTime Changed { get; set; } = DateTime.UtcNow;

    [Required] public int Version { get; set; } = 1;

    [Required] public bool Undeletable { get; set; }

    public bool StateOrRemove() => true;
}

public abstract class BaseModel<TDto> : BaseModel where TDto : new()
{
    public TDto ToDto() => new();
}