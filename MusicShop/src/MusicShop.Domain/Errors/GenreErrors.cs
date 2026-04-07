using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class GenreErrors
{
    public static readonly Error DuplicateSlug = new(
        "Genre.Conflict", 
        "A genre with the same slug already exists.");
        
    public static readonly Error NotFound = new(
        "Genre.NotFound", 
        "The specified genre was not found.");
}
