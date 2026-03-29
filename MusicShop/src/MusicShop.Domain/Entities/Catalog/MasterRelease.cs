using System.Collections.Generic;
using MusicShop.Domain.Common;


namespace MusicShop.Domain.Entities.Catalog;

/// <summary>
/// Original album - e.g.: "Dark Side of the Moon" (1973)
/// Separated from Release so one album can have multiple different pressings.
/// </summary>
public class MasterRelease : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string? Genre { get; set; }
    public string? CoverUrl { get; set; }
    public string? Description { get; set; }

    // FK
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; } = null!;

    // Navigation: 1 Master has many pressings (Releases)
    public ICollection<Release> Releases { get; set; } = new List<Release>();
}
