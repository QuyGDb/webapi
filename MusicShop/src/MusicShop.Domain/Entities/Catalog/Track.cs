using MusicShop.Domain.Common;

namespace MusicShop.Domain.Entities.Catalog;

public class Track : BaseEntity
{
    public int Position { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? Duration { get; set; } // seconds

    // FK
    public Guid ReleaseId { get; set; }
    public Release Release { get; set; } = null!;
}
