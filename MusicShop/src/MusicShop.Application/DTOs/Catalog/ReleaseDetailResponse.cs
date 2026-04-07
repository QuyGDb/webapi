namespace MusicShop.Application.DTOs.Catalog;

public class ReleaseDetailResponse : ReleaseResponse
{
    public List<TrackDto> Tracks { get; set; } = new();
    public List<ReleaseVersionDto> Versions { get; set; } = new();
}
