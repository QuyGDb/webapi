using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class MasterReleaseErrors
{
    public static readonly Error NotFound = new(
        "MasterRelease.NotFound", 
        "Bản phát hành gốc (Master Release) không tồn tại.");

    public static readonly Error ArtistNotFound = new(
        "MasterRelease.ArtistNotFound", 
        "Nghệ sĩ liên quan không tồn tại trong hệ thống.");
}
