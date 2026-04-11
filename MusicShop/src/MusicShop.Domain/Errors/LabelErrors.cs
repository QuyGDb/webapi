using MusicShop.Domain.Common;

namespace MusicShop.Domain.Errors;

public static class LabelErrors
{
    public static readonly Error NotFound = new(
        "Label.NotFound", 
        "The label with the specified identifier was not found.");

    public static readonly Error DuplicateName = new(
        "Label.DuplicateName", 
        "A label with the same name already exists.");

    public static readonly Error DuplicateSlug = new(
        "Label.DuplicateSlug", 
        "A label with the same slug already exists.");

    public static readonly Error HasAssociations = new(
        "Label.HasAssociations", 
        "Cannot delete label with existing release versions.");
}
