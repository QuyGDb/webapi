using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class LabelErrors
{
    public static readonly Error NotFound = new(
        "Label.NotFound", 
        "The label with the specified identifier was not found.");

    public static readonly Error DuplicateName = new(
        "Label.Conflict", 
        "A label with the same name already exists.");
}
