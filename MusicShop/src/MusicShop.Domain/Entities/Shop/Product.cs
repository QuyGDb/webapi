using System.Collections.Generic;
using MusicShop.Domain.Common;

using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Enums;

namespace MusicShop.Domain.Entities.Shop;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverUrl { get; set; }
    public ProductType Type { get; set; }
    public bool IsActive { get; set; } = true;

    // Limited Edition
    public bool IsLimitedEdition { get; set; }
    public int? LimitedQuantity { get; set; }

    // Pre-order
    public bool IsPreOrder { get; set; }
    public DateTime? ReleaseDate { get; set; }

    // FK (nullable - merch like shirts, books don't have a Release)
    public Guid? ReleaseId { get; set; }
    public Release? Release { get; set; }

    // Navigation
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductCollectionItem> CollectionItems { get; set; } = new List<ProductCollectionItem>();
}
