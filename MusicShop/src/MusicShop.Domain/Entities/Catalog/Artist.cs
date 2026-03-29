using System.Collections.Generic;
using MusicShop.Domain.Common;


namespace MusicShop.Domain.Entities.Catalog;

public class Artist : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Genre { get; set; }
    public string? Country { get; set; }
    public string? ImageUrl { get; set; }

    // Navigation: 1 Artist has many MasterReleases
    public ICollection<MasterRelease> MasterReleases { get; set; } = new List<MasterRelease>();
}
