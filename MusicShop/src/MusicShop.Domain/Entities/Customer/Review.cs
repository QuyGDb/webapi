using MusicShop.Domain.Common;

namespace MusicShop.Domain.Entities.Customer;

public class Review : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
