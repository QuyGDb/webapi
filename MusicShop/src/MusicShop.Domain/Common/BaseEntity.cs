namespace MusicShop.Domain.Common;

/// <summary>
/// Base class for all Entities. Every table in the DB has Id, CreatedAt, and UpdatedAt.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
