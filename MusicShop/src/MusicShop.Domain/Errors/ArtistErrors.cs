using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class ArtistErrors
{
    public static readonly Error NotFound = new(
        "Artist.NotFound", 
        "The artist with the specified identifier was not found.");

    public static readonly Error DuplicateName = new(
        "Artist.DuplicateName", 
        "An artist with the same name already exists.");

    public static readonly Error DuplicateSlug = new(
        "Artist.DuplicateSlug", 
        "An artist with the same slug already exists.");

    public static readonly Error HasAssociations = new(
        "Artist.HasAssociations", 
        "Cannot delete artist with existing releases.");
}
