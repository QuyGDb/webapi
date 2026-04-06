using MusicShop.Domain.Common;
using MusicShop.Domain.Enums;

namespace MusicShop.Domain.Entities.Customer;

public class Collection : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public decimal PurchasePrice { get; set; }
    public string? Notes { get; set; }
    public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;
}
