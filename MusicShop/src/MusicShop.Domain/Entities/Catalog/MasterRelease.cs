using System.Collections.Generic;
using MusicShop.Domain.Common;


namespace MusicShop.Domain.Entities.Catalog;

/// <summary>
/// Album gốc - ví dụ: "Dark Side of the Moon" (1973)
/// Tách khỏi Release để 1 album có thể có nhiều bản ép khác nhau.
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

    // Navigation: 1 Master có nhiều bản ép (Release)
    public ICollection<Release> Releases { get; set; } = new List<Release>();
}
