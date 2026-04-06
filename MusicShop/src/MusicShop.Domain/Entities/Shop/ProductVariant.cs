using MusicShop.Domain.Common;
using MusicShop.Domain.Enums;

namespace MusicShop.Domain.Entities.Shop;

public class ProductVariant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Metadata for physical media
    public string? Colorway { get; set; }

    // FK
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
