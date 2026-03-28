using MusicShop.Domain.Common;

namespace MusicShop.Domain.Entities.Customer;

public class Wishlist : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public bool NotifyByEmail { get; set; }
}
