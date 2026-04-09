namespace MusicShop.Application.DTOs.Catalog;

public class ReleaseDetailResponse : ReleaseResponse
{
    public ArtistResponse Artist { get; set; } = null!;
    public List<TrackDto> Tracks { get; set; } = new();
    public List<ReleaseVersionDto> Versions { get; set; } = new();
}
