using MusicShop.Domain.Common;

namespace MusicShop.Domain.Entities.System;

public class AdminActivityLog : BaseEntity
{
    public Guid AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? Detail { get; set; }
}
