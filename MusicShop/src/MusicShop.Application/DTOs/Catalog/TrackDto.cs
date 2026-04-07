namespace MusicShop.Application.DTOs.Catalog;

public class TrackDto
{
    public Guid Id { get; set; }
    public int Position { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? DurationSeconds { get; set; }
    public string? Side { get; set; } // A, B, etc.
}
