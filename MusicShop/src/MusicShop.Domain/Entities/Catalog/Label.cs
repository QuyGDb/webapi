using System.Collections.Generic;
using MusicShop.Domain.Common;


namespace MusicShop.Domain.Entities.Catalog;

public class Label : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
    public string? Website { get; set; }

    // Navigation: 1 Label phát hành nhiều Release
    public ICollection<Release> Releases { get; set; } = new List<Release>();
}
