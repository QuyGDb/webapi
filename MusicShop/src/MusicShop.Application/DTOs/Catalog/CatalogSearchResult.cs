using System.Collections.Generic;

namespace MusicShop.Application.DTOs.Catalog;

public class CatalogSearchResult
{
    public List<ArtistResponse> Artists { get; set; } = new();
    public List<ReleaseResponse> Releases { get; set; } = new();
}
