using System.Collections.Generic;
using MusicShop.Domain.Common;

using MusicShop.Domain.Enums;

namespace MusicShop.Domain.Entities.Catalog;

/// <summary>
/// Specific pressing - e.g.: "Dark Side of the Moon - US 1973 first press"
/// or "Japan OBI 1976", "2011 remaster 180g"...
/// </summary>
public class Release : BaseEntity
{
    public string? Country { get; set; }
    public int? Year { get; set; }
    public ReleaseFormat Format { get; set; }
    public string? CatalogNumber { get; set; }

    // FK
    public Guid MasterId { get; set; }
    public MasterRelease Master { get; set; } = null!;

    public Guid LabelId { get; set; }
    public Label Label { get; set; } = null!;

    // Navigation
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
