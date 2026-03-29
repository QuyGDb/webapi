using MusicShop.Domain.Common;
using MusicShop.Domain.Enums;


namespace MusicShop.Domain.Entities.Shop;

public class PriceHistory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public Condition? Condition { get; set; }
    public decimal Price { get; set; }
    public DateTime SoldAt { get; set; } = DateTime.UtcNow;
}
