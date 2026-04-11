namespace MusicShop.Application.DTOs.Catalog;

public class LabelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Country { get; set; }
    public int? FoundedYear { get; set; }
    public string? Website { get; set; }
}
