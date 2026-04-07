using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class ReleaseErrors
{
    public static readonly Error NotFound = new(
        "Release.NotFound", 
        "The specified release was not found.");

    public static readonly Error ArtistNotFound = new(
        "Artist.NotFound", 
        "The associated artist was not found.");
}
