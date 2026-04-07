using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class ArtistErrors
{
    public static readonly Error NotFound = new(
        "Artist.NotFound", 
        "The artist with the specified identifier was not found.");

    public static readonly Error DuplicateName = new(
        "Artist.Conflict", 
        "An artist with the same name already exists.");
}
