using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class ReleaseErrors
{
    public static readonly Error NotFound = new(
        "Release.NotFound", 
        "Bản phát hành gốc (Release) không tồn tại.");

    public static readonly Error ArtistNotFound = new(
        "Artist.NotFound", 
        "Nghệ sĩ liên quan không tồn tại trong hệ thống.");
}
