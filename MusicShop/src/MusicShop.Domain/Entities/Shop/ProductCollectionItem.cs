namespace MusicShop.Domain.Entities.Shop;

/// <summary>
/// Junction table for M:N relationship between Product and ProductCollection
/// </summary>
public class ProductCollectionItem
{
    public Guid CollectionId { get; set; }
    public ProductCollection Collection { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int SortOrder { get; set; }
}
