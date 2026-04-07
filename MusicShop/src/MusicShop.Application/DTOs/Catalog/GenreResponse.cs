namespace MusicShop.Application.DTOs.Catalog;

public class GenreResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
