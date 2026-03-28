using System.Collections.Generic;
using MusicShop.Domain.Common;


namespace MusicShop.Domain.Entities.Shop;

public class ProductCollection : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverUrl { get; set; }
    public int SortOrder { get; set; }

    public ICollection<ProductCollectionItem> Items { get; set; } = new List<ProductCollectionItem>();
}
